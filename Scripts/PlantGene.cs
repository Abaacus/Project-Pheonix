using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gene", menuName = "Plants/Genes")]
public class PlantGene : ScriptableObject
{
    public float[] growthTimes = new float[5];
    public float[] witherTimes = new float[5];
    public Sprite[] growthSprites;
    public Sprite[] witherSprites;

    public int[] mutateChanceWeights;
    public Item[] mutatables;

    public float dropRate;
}
