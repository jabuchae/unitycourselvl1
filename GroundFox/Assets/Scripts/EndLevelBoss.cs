﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelBoss : MonoBehaviour
{
    [SerializeField] private GameObject minePrefab;
    [SerializeField] private float timeBetweenMines = 3f;
    [SerializeField] private float difficultyRamp = 0.05f;

    private float mineTime;

    private void Start()
    {
        mineTime = timeBetweenMines;
    }

    void Update()
    {
        mineTime -= Time.deltaTime;

        if (mineTime <= 0)
        {
            mineTime = timeBetweenMines;
            if (timeBetweenMines >= 1f)
            {
                timeBetweenMines -= difficultyRamp;
            }
            
            CreateMine();
        }
    }

    private void CreateMine()
    {
        var mine = Instantiate<GameObject>(minePrefab, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
        mine.AddComponent<SelfDestruct>();
    }
}
