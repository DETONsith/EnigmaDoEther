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
    int gamestate;
    int fase2counter;

    //limitadores de input
    private bool canswipe = false;
    private bool caninteract = false;
    private bool canlookup = false;



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
    public AudioClip[] AudioEnigma1 = { }; // 0 = audio abriu palha pegou isqueiro. | 1 = audio madeira pegou fogo surgiu martelo | 2 = audio martelo quebrou vidro surgiu chave | 3 = audio falha no 0 | 4 - audio falha reset
    public AudioClip[] AudioTutorial = { }; // 0 = rockmove | 1 = rockplace
    public AudioClip[] AmbientClips = { };
    



    //variáveis da gambiarra e da vergonha
    private bool gamestarted;
    private bool vt_swipe;
    private bool vt_interact;
    private bool vt_lookup;
    private bool vt_tutorial_solved;
    private bool vt_fase1_solved;
    private bool vt_fase2_solved;
    private bool sair;



    private void Start()
    { 
        //inicializa as variáveis
        fase = 0; //substituir para carregar de onde parou
        objcarry = 0;
        carrying = false;
        CurrentPosition = 0;
        gamestate = 0;
        fase2counter = 0;

        //variaveis da tristeza
        gamestarted = false;
        vt_swipe = false;
        vt_interact = false;
        vt_lookup = false;
        vt_tutorial_solved = false;
        vt_fase1_solved = false;
        vt_fase2_solved = false;
        sair = false;

        if (fase == 0)
        {
            
            UpdateObjetos();
        }
        else
        {
            UpdateObjetos();
        }

    }

    private void Update()
    {
        stageflow();
        Swipe();
    }

    private void stageflow()
    {
        if (gamestate == 0)
        {
            // AudioDialog.clip = (audio confirmação iniciar jogo);
            AudioDialog.Play();
            gamestate = 1;
        }
        else if(gamestate == 1 && gamestarted == true)
        {
            AudioDialog.Stop();
            //AudioDialog.clip = (audio introdução + fala pra swipe)
            AudioDialog.Play();
            if (AudioAmbient.isPlaying)
            {
                AudioAmbient.Stop();
            }
            AudioAmbient.clip = AmbientClips[0];
            AudioAmbient.Play();
            gamestate = 2;
            canswipe = true;
        }
        else if (gamestate == 2 && vt_swipe == true) {
            if (AudioDialog.isPlaying == false)
            {
                AudioDialog.Stop();
                //AudioDialog.clip = (audio fala pra interagir)
                AudioDialog.Play();
                gamestate = 3;
            }
        }
        else if (gamestate == 3 && vt_interact == true)
        { if (AudioDialog.isPlaying == false)
            {
                AudioDialog.Stop();
                //AudioDialog.clip = (audio fala pra lookup)
                AudioDialog.Play();
                gamestate = 4;
            }
        }
        else if (gamestate == 4 && vt_lookup == true)
        { if (AudioDialog.isPlaying == false)
            {
                AudioDialog.Stop();
                //AudioDialog.clip = (audio fala pra resolver o enigma)
                AudioDialog.Play();
                gamestate = 5;
            }
        }
        else if (gamestate == 5 && vt_tutorial_solved == true)
        {
            if (AudioDialog.isPlaying == false)
            {
                AudioDialog.Stop();
                //AudioDialog.clip = (Audio finalização tutorial e introdução fase 1)
                AudioDialog.Play();

                if (AudioAmbient.isPlaying)
                {
                    AudioAmbient.Stop();
                }
                AudioAmbient.clip = AmbientClips[1];
                AudioAmbient.Play();

                gamestate = 6;
                fase = 1;
                UpdateObjetos();
            }
        }
        else if (gamestate == 6 && vt_fase1_solved == true)
        {
            if (AudioDialog.isPlaying == false)
            {
                AudioDialog.Stop();
                //AudioDialog.clip = (Audio finalização fase 1 e introdução fase 2)
                AudioDialog.Play();
                if (AudioAmbient.isPlaying)
                {
                    AudioAmbient.Stop();
                }
                AudioAmbient.clip = AmbientClips[2];
                AudioAmbient.Play();
                gamestate = 7;
            }
            
        }
        else if (gamestate == 7 && vt_fase2_solved == true)
        {
            if (AudioDialog.isPlaying == false)
            {
                AudioDialog.Stop();
                if (AudioAmbient.isPlaying)
                {
                    AudioAmbient.Stop();
                }
                //AudioDialog.clip = (Audio finalização fase 2, agradecimento, toque simples para sair )
                AudioDialog.Play();
                gamestate = 8;
                caninteract = false;
                canlookup = false;
                canswipe = false;
            }
        }
        else if (gamestate == 8 && sair == true)
        {
            Application.Quit();
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
        if (fase == 0)
        {
            Interativo[] tempobj = { objFase[0],objFase[1],objFase[2],objFase[3]};

            foreach (Interativo it in objFase)
            {
                if (it.getValue() == "2")
                {
                    tempobj[0] = it;
                }
                else if (it.getValue() == "4")
                {
                    tempobj[1] = it;
                }
                else if (it.getValue() == "8")
                {
                    tempobj[2] = it;
                }
                else if (it.getValue() == "16")
                {
                    tempobj[3] = it;
                }
            }

            objFase[0] = tempobj[1]; // item 1 = 4
            objFase[1] = tempobj[0]; // item 2 = 2
            objFase[2] = tempobj[3]; // item 3 = 16
            objFase[3] = tempobj[2]; // item 4 = 8

            Debug.Log(objFase[0].getValue() + "-" + objFase[1].getValue() + "-" + objFase[2].getValue() + "-" + objFase[3].getValue()); //DEBUG 


        }
        if (fase == 1)
        {
            Interativo[] tempobj = { objFase[0], objFase[1], objFase[2], objFase[3] };

            foreach (Interativo it in objFase)
            {
                if (it.getValue() == "palha")
                {
                    tempobj[0] = it;
                }
                else if (it.getValue() == "madeira")
                {
                    tempobj[1] = it;
                }
                else if (it.getValue() == "vidro")
                {
                    tempobj[2] = it;
                }
                else if (it.getValue() == "metal")
                {
                    tempobj[3] = it;
                }
            }

            objFase[0] = tempobj[1]; // item 1 = madeira
            objFase[1] = tempobj[0]; // item 2 = palha
            objFase[2] = tempobj[3]; // item 3 = metal
            objFase[3] = tempobj[2]; // item 4 = vidro

            Debug.Log(objFase[0].getValue() + "-" + objFase[1].getValue() + "-" + objFase[2].getValue() + "-" + objFase[3].getValue()); //DEBUG 
        }
        
        
        /*   |CASO QUEIRA RANDOMIZAÇÃO AQUI ESTÁ O CÓDIGO PRONTO|
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
        }*/
    }


    private void moveLeft() //swipe left
    {
        if (canswipe == true)
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
        if (gamestate == 3) //                                        
        {
            vt_swipe = true;
        }
        
    }

    private void moveRight() //swipe right
    {
        if (canswipe == true)
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
    }

    private void interact()
    {
        if (gamestate == 1)
        {
            gamestarted = true;
        }
        else if(gamestate == 3)                                 
        {
            vt_interact = true;
        }
        else if (gamestate == 8)
        {
            sair = true;
        }

        if (caninteract == true)
        {
            switch (fase)
            {
                case 0:
                    if (carrying == false)
                    {
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
                    switch (fase2counter)
                    {
                        case 0:
                            {
                                if (objFase[CurrentPosition].value == "palha")
                                {
                                    fase2counter = 1;
                                    AudioDialog.clip = AudioEnigma1[0]; // conseguiu, dentro da palha tem isqueiro.
                                    AudioDialog.Play();
                                }
                                else
                                {
                                    AudioDialog.clip = AudioEnigma1[3]; // falhou no 0
                                    AudioDialog.Play();
                                }
                                break;
                            }
                        case 1:
                            {
                                if (objFase[CurrentPosition].value == "madeira")
                                {
                                    fase2counter = 2;
                                    AudioDialog.clip = AudioEnigma1[1]; // conseguiu, queimou madeira surgiu martelo
                                    AudioDialog.Play();
                                }
                                else
                                {
                                    AudioDialog.clip = AudioEnigma1[4]; // oh não, as caixas se restauraram e os itens sumiram!
                                    AudioDialog.Play();
                                    fase2counter = 0;
                                }
                                break;
                            }
                        case 2:
                            {
                                if (objFase[CurrentPosition].value == "vidro")
                                {
                                    fase2counter = 3;
                                    AudioDialog.clip = AudioEnigma1[2]; // conseguiu, quebrou vidro e pegou a chave dentro
                                    AudioDialog.Play();
                                }
                                else
                                {
                                    AudioDialog.clip = AudioEnigma1[4]; // oh não, as caixas se restauraram e os itens sumiram!
                                    AudioDialog.Play();
                                    fase2counter = 0;
                                }
                                break;
                            }
                        case 3:
                            {
                                if (objFase[CurrentPosition].value == "metal")
                                {
                                    fase2counter = 4;
                                }
                                else
                                {
                                    AudioDialog.clip = AudioEnigma1[4]; // oh não, as caixas se restauraram e os itens sumiram!
                                    AudioDialog.Play();
                                    fase2counter = 0;
                                }
                                break;
                            }
                    }
                    WinCondition(fase);
                    break;
                case 2:
                    break;
            }
        }
    }

    private void lookup()
    {

        if (canlookup == true)
        {

            AudioDialog.Stop();
            
           
            //5 é liberado para outro lookup
                AudioDialog.clip = lookupaudios[fase];
                AudioDialog.Play();
                StartCoroutine(lookuprotina()); //toca audio da fase inteiro


            AudioDialog.Stop();
            AudioDialog.clip = objFase[0].audSolo;
                    AudioDialog.Play();

            StartCoroutine(lookuprotina()); //toca audio do obj 1


            AudioDialog.Stop();
            AudioDialog.clip = objFase[1].audSolo; 
                    AudioDialog.Play();

            StartCoroutine(lookuprotina()); // toca audio do objeto 2

            AudioDialog.Stop();
            AudioDialog.clip = objFase[2].audSolo; 
                    AudioDialog.Play();

            StartCoroutine(lookuprotina()); // toca audio do objeto 3

            AudioDialog.Stop();
            AudioDialog.clip = objFase[3].audSolo; 
                    AudioDialog.Play();

            StartCoroutine(lookuprotina()); // toca audio do objeto 4


        }
    }

    IEnumerator lookuprotina()
    {
        yield return new WaitForSeconds(AudioDialog.clip.length);
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
                        gamestate = 5;
                        vt_tutorial_solved = true;
                    }
                    break;
                }
            case 1:
                {
                    if(fase2counter == 4)
                    {
                        gamestate = 6;
                        vt_fase1_solved = true;
                    }
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
                if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
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
                if (currentSwipe.x == 0 && currentSwipe.y == 0)
                {
                    
                    Debug.Log("single tocuh");
                }
            }
        }
        
    }
}

