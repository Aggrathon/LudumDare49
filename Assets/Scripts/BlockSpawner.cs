using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] List<SirtetBlock> blocks;
    SirtetBlock current;
    int index;

    public bool multiple = false;
    public bool rotate = false;
    public float minDist = 8f;

    void Start()
    {
        index = blocks.Count;
        current = null;
    }


    void Update()
    {
        if (!current || ((multiple || !current.enabled) && Vector3.SqrMagnitude(transform.position - current.transform.position) > minDist * minDist))
        {
            if (index >= blocks.Count)
            {
                blocks.Shuffle();
                index = 0;
            }
            if (rotate)
            {
                Quaternion rot = Quaternion.Euler(0f, 0f, Random.Range(0, 4) * 90f);
                current = Instantiate(blocks[index], transform.position, rot);
            }
            else
            {
                current = Instantiate(blocks[index], transform.position, transform.rotation);
            }
            index++;
        }
        else
        {
            // TODO move rope
        }
    }
}