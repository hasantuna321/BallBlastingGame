using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class BallController : MonoBehaviour
{
    public enum ballState
    {
        //oyun ilerleyi�ine g�re oyuncunun o anki durumunu �zetleyen de�i�kenler (ref1)
        aim,
        fire,
        wait,
        endShot,
        endGame,
        pauseGame,
    }
    public ballState currentBallState;
    public Rigidbody2D ball;
    private Vector2 mouseStartPosition;
    private Vector2 mouseEndPosition;
    public Vector2 tempVelocity;
    public Vector3 ballLaunchPosition;
    private float ballVelocityX;
    private float ballVelocityY;
    public float constantSpeed;
    public GameObject arrow;
    private GameManager gameManager;
    public GameObject pausePanel;  // Pause paneli referans�
    public bool isPaused = false;

    void Start()
    {
        // Layer indexlerini al�n
        int Ball = LayerMask.NameToLayer("Ball");
        int Ball1 = LayerMask.NameToLayer("Ball");

        // Bu iki layer'�n �arp��mas�n� engelle
        Physics2D.IgnoreLayerCollision(Ball, Ball1);

        gameManager = FindObjectOfType<GameManager>();
        currentBallState = ballState.aim;
        gameManager.ballsInScene.Add(this.gameObject);

    }


    void Update()
    {

        switch (currentBallState)
        {
            //(ref1)  de�i�kenlerin oyun i�erisinde g�r�nmesini sa�layan caseler 
            case ballState.aim:
                //kontrollere g�re fonksiyonlar� �a��ran k�s�m




                if (Input.GetMouseButtonDown(0))
                {
                    MouseClicked();
                }
                if (Input.GetMouseButton(0))
                {
                    MouseDragged();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    RelaseMouse();
                }


                break;

            case ballState.fire:
                arrow.SetActive(false);
                break;

            case ballState.wait:
                
                if (gameManager.ballsInScene.Count == 1)
                {
                    currentBallState = ballState.endShot;

                }
                break;

            case ballState.endShot:
                for (int i = 0; i < gameManager.bricksInScene.Count; i++)
                {
                    gameManager.bricksInScene[i].GetComponent<BrickMovementController>().currentState = BrickMovementController.brickState.move;
                }
                gameManager.PlaceBricks();
                currentBallState = ballState.aim;
                break;

            case ballState.endGame:
                break;

            case ballState.pauseGame:
                Time.timeScale = 0f;  // Oyunu durdur

                break;

            default:
                break;
        }
    }

   
    public void MouseClicked()
    {
        //mouse ile t�kland���nda (mobilde ekrana dokunuldu�unda) ba�layan pozisyonu a�� ba�lang�c� olarak alan fonksiyon

        mouseStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(mouseStartPosition); // mouse un o anki koordinatlar�n� g�steren debugger
    }

    public void MouseDragged()
    {
        //s�r�kleme yap�ld�k�a ilk referans a��dan d�z bir �ekilde di�er a��y� olu�turmaya yarayan fonksiyon

        arrow.SetActive(true);
        Vector2 tempMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float diffX = mouseStartPosition.x - tempMousePosition.x;
        float diffY = mouseStartPosition.y - tempMousePosition.y;
        //c# ile olu�turdu�umuz a��n�n unity taraf�ndan anla��lmas� ad�na ve e�er t�klanan ve s�r�klenen k�s�m ayn� d�zlemde olursa olu�abilecek hatay� a�mak ad�na eklenen ko�ul
        if (diffY <= 0)
        {
            diffY = .01f;
        }
        float theta = Mathf.Rad2Deg * Mathf.Atan(diffX / diffY);
        arrow.transform.rotation = Quaternion.Euler(0f, 0f, -theta);
        //Oku Hareket Ettirme K�sm�
    }

    public void RelaseMouse()
    { 
        //mouse b�rak�ld���nda, b�rak�ld��� konumu referans alacak fonksiyon
        arrow.SetActive(false);
        mouseEndPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ballVelocityX = (mouseStartPosition.x - mouseEndPosition.x);
        ballVelocityY = (mouseStartPosition.y - mouseEndPosition.y);
        tempVelocity = new Vector2(ballVelocityX, ballVelocityY).normalized;
        ball.velocity = constantSpeed * tempVelocity;
        //e�er ekrana bo� t�klan�rsa (ki bu durumda ekranda hi� bir a�� olmayaca��ndan f�rlatma a��s� olu�mayacak ve f�rlatma ger�ekle�meyecek) d�ng�y� tekrarlamas�n� sa�layan ko�ul
        if (ball.velocity == Vector2.zero)
        {
            return;
        }
        ballLaunchPosition = transform.position;
        currentBallState = ballState.fire;
    }

}