using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    List<GameObject> Entities = new List<GameObject>();
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        while (i < 100)
        {
            GameObject entity = Instantiate(prefab, transform.position, prefab.transform.rotation);
            entity.GetComponent<InputsOutputsController>().spawner = this;
            entity.name = i.ToString();
            Entities.Add(entity);
            i++;
        }
    }

    public void CheckEntitiesLife()
    {
        foreach(GameObject entity in Entities)
        {
            if(entity.GetComponent<InputsOutputsController>().speed > 0)
            {
                return;
            }
        }
        MutateAndResetEntities();
    }
    public void MutateAndResetEntities()
    {
        Debug.Log("Mutate");
        NeuralNetwork bestBrain = null;
        string bestEntityName = "";
        GameObject bestEntity = null;
        float maxvalue = 0;
        foreach (GameObject entity in Entities)
        {
            if (entity.GetComponent<InputsOutputsController>().fit > maxvalue)
            {
                maxvalue = entity.GetComponent<InputsOutputsController>().fit;
                bestEntityName = entity.name;
                bestEntity = entity;
            }
        }
        bestBrain = bestEntity.GetComponent<Brain>().neuralNetwork;
        foreach (GameObject entity in Entities)
        {
            if (entity.name != bestEntityName)
            {
                Debug.Log("Neuron Does  Mutate");
                entity.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                entity.GetComponent<Brain>().neuralNetwork.changeNetwork(bestBrain);
                entity.GetComponent<Brain>().neuralNetwork.Mutate();
                entity.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            else
            {
                Debug.Log("Best Neuron Does Not Mutate");
                entity.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                entity.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            entity.GetComponent<InputsOutputsController>().ResetEntity(); 
        }
    }
}
