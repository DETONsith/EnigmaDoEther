using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regrador : MonoBehaviour
{
    int fase;
    int CurrentPosition;
    bool carrying;
    bool[] lookupaudioBreaker = { };
    int objcarry;




    //link de objetos
    public List<Interativo> objetos;
    public List<Interativo> objFase;
    public AudioSource AudioDialog;
    public AudioSource AudioAmbient;
    public AudioClip TutorialMoveInteract;
    public AudioClip a1;
    private string[] Solutions = { "2-4-8-16", "resposta fase 1", "resposta fase 2" };

    //audios de interação:
    public AudioClip[] lookupaudios = { }; //colocar aqui audios de lookup | 0 = tutorial | 1 = fase 1 | 2 = fase 2 |
    public AudioClip[] AudioEnigma2 = { }; //colocar aqui audios de interação com objeto do enigma 2
    public AudioClip[] AudioEnigma1 = { };
    public AudioClip[] AudioTutorial = { }; // 0 = rockmove | 1 = rockplace




    private void Start()
    { 
        //inicializa as variáveis
        fase = 0; //substituir para carregar de onde parou
        objcarry = 0;
        carrying = false;
        CurrentPosition = 0;
        if (fase == 0)
        {
            //função que inicializa tutorial
            UpdateObjetos();
        }
        else
        {
            UpdateObjetos();
        }

    }


    private void UpdateObjetos() //cria a lista de objetos da fase atual
    {

        foreach (Interativo intera in objetos)
        {
            if (intera != null) //pega objetos existentes
            {
                if (intera.getLevel() == fase) //pega objetos da fase atual
                {
                    objFase.Add(intera); // adiciona a lista de objetos da fase atual

                }
            }
        }
            randomizer();

    }

    private void randomizer()
    {
        for (int i = 0; i < objFase.Count; i++) //randomiza ordem dos objetos
        {

            Interativo temp = objFase[i];
            int randomIndex = Random.Range(i, objFase.Count);
            objFase[i] = objFase[randomIndex];
            objFase[randomIndex] = temp;
        }

        Debug.Log(objFase[0].getValue() + "-" + objFase[1].getValue() + "-" + objFase[2].getValue() + "-" + objFase[3].getValue()); //DEBUG 


        string resultado = objFase[0].getValue() + "-" + objFase[1].getValue() + "-" + objFase[2].getValue() + "-" + objFase[3].getValue();
        if (resultado == "2-4-8-16")
        {
            Debug.Log("REINICIANDO RANDOMIZAÇÃO"); //debug
            randomizer();
        }
    }


    private void moveLeft() //swipe left
    {
        if (CurrentPosition == 0) { CurrentPosition = 3; }
        else
        {
            CurrentPosition -= 1;
        }
        AudioDialog.Stop();
        AudioDialog.clip = objFase[CurrentPosition].getLook();
        AudioDialog.Play();
    }

    private void moveRight() //swipe left
    {
        if (CurrentPosition == 3) { CurrentPosition = 0; }
        else
        {
            CurrentPosition += 1;
        }
        AudioDialog.Stop();
        AudioDialog.clip = objFase[CurrentPosition].getLook();
        AudioDialog.Play();
    }

    

    private void interact()
    {
        switch (fase)
        {
            case 0:
                if (carrying == false) {
                    objcarry = CurrentPosition;
                    carrying = true;
                    AudioDialog.clip = TutorialMoveInteract; //som de mover objeto no tutorial
                    WinCondition(fase);
                }
                else
                {
                    carrying = false;
                    Interativo temp = objFase[CurrentPosition];
                    objFase[CurrentPosition] = objFase[objcarry];
                    objFase[objcarry] = temp;
                    WinCondition(fase);
                }
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }

    private void lookup()
    {
        if (lookupaudioBreaker[5] == true)
        { //5 é liberado para outro lookup
            AudioDialog.clip = lookupaudios[fase];
            AudioDialog.Play();
            lookupaudioBreaker[5] = false;
        }
        else
        {
            if (lookupaudioBreaker[0] == true)  { //caso tenha finalizado o audio 0 o audiobreaker fica true
               AudioDialog.clip = objFase[0].audSolo;
                AudioDialog.Play();
                lookupaudioBreaker[0] = false;
               
            }
            else if (lookupaudioBreaker[1] == true)
            { //caso tenha finalizado o audio 0 o audiobreaker fica true
                AudioDialog.clip = objFase[1].audSolo;
                AudioDialog.Play();
                lookupaudioBreaker[1] = false;

            }
            else if (lookupaudioBreaker[2] == true)
            { //caso tenha finalizado o audio 0 o audiobreaker fica true
                AudioDialog.clip = objFase[2].audSolo;
                AudioDialog.Play();
                lookupaudioBreaker[2] = false;
            }
            else if (lookupaudioBreaker[3] == true)
            { //caso tenha finalizado o audio 0 o audiobreaker fica true
                AudioDialog.clip = objFase[3].audSolo;
                AudioDialog.Play();
                lookupaudioBreaker[3] = false;

            }
        }
    }








    private void WinCondition(int level)
    {
        switch (level)
        {

            case 0:
                {
                    string testeresultado = objFase[0].getValue() + "-" + objFase[1].getValue() + "-" + objFase[2].getValue() + "-" + objFase[3].getValue();
                    if (testeresultado == "2-4-8-16")
                    {

                    }
                    break;
                }
            case 1:
                {

                    break;
                }
        }
    }








    // --------------------------------------------
    // | Área do stackoverflow | Sistema de SWIPE |
    // --------------------------------------------

    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    public void Swipe() //OBS: COLOCAR NO UPDATE PARA FUNCIONAR
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                //save began touch 2d point
                firstPressPos = new Vector2(t.position.x, t.position.y);
            }
            if (t.phase == TouchPhase.Ended)
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(t.position.x, t.position.y);

                //create vector from the two points
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                //normalize the 2d vector
                currentSwipe.Normalize();

                //swipe upwards
                if (currentSwipe.y > 0  && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    lookup();
                    Debug.Log("up swipe");
                }
                //swipe down
                if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    Debug.Log("down swipe");
                }
                //swipe left
                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    moveLeft();
                    Debug.Log("left swipe");
                }
                //swipe right
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    moveRight();
                    Debug.Log("right swipe");
                }
            }
        }
    }
}

