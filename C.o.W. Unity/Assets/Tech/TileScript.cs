using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TileScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] GameObject _objectPlayer;
    [SerializeField] PlayerScript _player;
    [SerializeField] HoldingScript _holdingScript;
    public bool hasPlayer;

    public Vector2 tilePos;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        tilePos = new Vector2(this.transform.position.x, this.transform.position.z);
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        meshRenderer.material.color = Color.green;
        //Debug.Log("Hello");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        meshRenderer.material.color = Color.white;
        //Debug.Log("Goodbye");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(hasPlayer && !_holdingScript.isHoldingPlayer)
        {
            _holdingScript.isHoldingPlayer = true;
        }
        Debug.Log("Clicked tile = " + tilePos);
    }

    
}
