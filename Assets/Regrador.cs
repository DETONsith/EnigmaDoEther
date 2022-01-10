using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regrador : MonoBehaviour
{
    int fase;
    int CurrentPosition;


    //link de objetos
    public List<Interativo> objetos;
    public List<Interativo> objFase;
    public AudioClip a1;
    private string[] Solutions = { "2-4-8-16", "resposta fase 1", "resposta fase 2" };


    private void Start()
    { 
        //inicializa as variáveis
        fase = 0; //substituir para carregar de onde parou
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

    private void moveLeft() //swipe left
    {
        if (CurrentPosition == 0) { CurrentPosition = 3; }
        else
        {
            CurrentPosition -= 1;
        }
        //colocar script de tocar o som
    }

    private void moveRight() //swipe left
    {
        if (CurrentPosition == 3) { CurrentPosition = 0; }
        else
        {
            CurrentPosition += 1;
        }
        //colocar script de tocar o som
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
        if (resultado == "2-4-8-16") { Debug.Log("REINICIANDO RANDOMIZAÇÃO"); //debug
            randomizer();
        }
    }

    private void WinCondition(int level)
    {

    }
}

