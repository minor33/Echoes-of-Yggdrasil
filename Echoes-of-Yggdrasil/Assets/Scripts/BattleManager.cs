using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject cardPrefab;

    public CardData cardData;

    public Transform cardPos;

    void Awake() {
        GameObject card = Instantiate(cardPrefab, cardPos.position, Quaternion.identity, cardPos);
        card.GetComponent<CardDisplay>().card = new Card(cardData);
    }
}
