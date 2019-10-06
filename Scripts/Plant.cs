using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plant : MonoBehaviour
{
    public PlantGene gene;
    public GameObject thirstIcon;
    public SpriteRenderer sprite;

    public enum PlantState { well, thirsty, dry, grown};
    public PlantState plantState;

    public Vector3 dropOffset = new Vector2(0, -1f);
    public float minDropRadius = 0.5f;
    public float maxDropRadius = 1f;
    float decayTime;
    float growthTime;
    int currentGrowth;
    public bool isImmortal;

    void Awake()
    {
        currentGrowth = 0;
        thirstIcon.SetActive(false);
        plantState = PlantState.well;
    }

    void Update()
    {
        switch (plantState)
        {
            case PlantState.well:
                thirstIcon.SetActive(false);
                sprite.sprite = gene.growthSprites[currentGrowth];
                growthTime += Time.deltaTime;
                if (growthTime >= gene.growthTimes[currentGrowth])
                {
                    if (currentGrowth < gene.growthTimes.Length-1)
                    {
                        plantState = PlantState.thirsty;
                        thirstIcon.SetActive(true);
                        currentGrowth++;
                        growthTime = 0f;
                    }
                    else
                    {
                        plantState = PlantState.grown;
                        transform.position += Vector3.back;
                    }
                }
                break;

            case PlantState.thirsty:
                if (currentGrowth > -1f)
                {
                    decayTime += Time.deltaTime;
                }
                if (decayTime >= gene.witherTimes[currentGrowth])
                {
                    plantState = PlantState.dry;
                }
                break;

            case PlantState.dry:
                if (isImmortal)
                {
                    plantState = PlantState.thirsty;
                    thirstIcon.SetActive(true);
                    decayTime = 0f;
                }
                else
                {
                    thirstIcon.SetActive(false);
                    sprite.sprite = gene.witherSprites[currentGrowth];
                }
                break;

            case PlantState.grown:
                growthTime += Time.deltaTime;
                if (growthTime >= gene.dropRate)
                {
                    Debug.Log("Dropping Seed");
                    Item randomSeed = GenerateRandomDrop();
                    if (randomSeed != null)
                    {
                        Vector3 randomPos = GenerateRandomPos();
                        Debug.Log(randomSeed);
                        Instantiate(randomSeed.prefab, randomPos, Quaternion.identity);
                        growthTime = 0f;
                    }
                }
                break;
        }
    }

    Vector3 GenerateRandomPos()
    {
        float wallRadius = (maxDropRadius - minDropRadius) * 0.5f;
        float ringRadius = wallRadius + minDropRadius;

        float rndAngle = Random.value * 6.28f;

        float cX = Mathf.Sin(rndAngle);
        float cY = Mathf.Cos(rndAngle);

        Vector3 ringPos = new Vector3(cX, cY, 0);
        ringPos *= ringRadius;
        Vector3 sPos = Random.insideUnitSphere * wallRadius;

        return ringPos + sPos + transform.position + dropOffset;
    }

    Item GenerateRandomDrop()
    {
        int roll = Random.Range(0, 101);
        int weightSum = 0;
        for (int i = 0; i < gene.mutateChanceWeights.Length; i++)
        {
            weightSum += gene.mutateChanceWeights[i];
            if (roll < weightSum)
            {
                return gene.mutatables[i];
            }
        }

        return null;
    }

    public void OnWater()
    {
        if (plantState == PlantState.thirsty)
        {
            Debug.Log("Plant Watered");
            plantState = PlantState.well;
            decayTime = 0f;
        }

        if (Random.Range(0, 2) > 0)
        {
            sprite.flipX = !sprite.flipX;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + dropOffset, minDropRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + dropOffset, maxDropRadius);
    }

}
