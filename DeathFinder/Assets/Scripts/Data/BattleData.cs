using PixelSilo.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : MonoBehaviour
{
      [Header("Turn")]
      public float _turnPeriod;     // 턴주기
      public int _turnStack;        // 현재턴 스택
      public int _turnLevel;        // 턴 속도 레벨
      public float _deckPeriod;     // 덱 버튼 활성화 시간

      [Header("Mana")]
      public int _manaStart;                    // 시작 마나
      public int _manaRestore;            // 턴당 마나 수급량
      public int _manaRestoreLevel;       // 마나 수급량 레벨
      public int _maxMana;                      // 최대 마나
      public int _maxManaLevel;           // 최대 마나 레벨
      public int _currentMana;                  //현재 마나

      [Header("Draw")]
      public int _drawCount;                    // 턴당 드로우 수
      public int _drawLevel;                    // 드로우 레벨
      public int _drawMaxHand;                  // 최대 핸드 수
      public int _drawMaxHandLevel;             // 최대 핸드 레벨

      [Header("Stack")]
      public int _stackChaos;                   // 카오스 스택



      // Start is called before the first frame update
      void Start()
      {
        
      }

      // Update is called once per frame
      void Update()
      {

      }
}
