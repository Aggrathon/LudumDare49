using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] List<SirtetBlock> blocks;
    SirtetBlock current;
    int index;

    void Start()
    {
        index = blocks.Count;
        current = null;
    }


    void Update()
    {
        if (!current || (!current.enabled && Vector3.SqrMagnitude(transform.position - current.transform.position) > 20))
        {
            if (index >= blocks.Count)
            {
                blocks.Shuffle();
                // TODO shuffle list
                index = 0;
            }
            current = Instantiate(blocks[index], transform.position, transform.rotation);
            index++;
        }
        else
        {
            // TODO move rope
        }
    }
}
