using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePortal : MonoBehaviour
{
    [SerializeField] private string destination;

    private bool inPortal = false;

    private void Update(){
        if(inPortal){
            if(Input.GetKeyDown(KeyCode.E)){
                switch(destination){
                    case "Passive":
                        BookManager.Instance.SetRoomPassive();
                        break;
                    case "Europe":
                        BookManager.Instance.SetRoomEurope();
                        break;
                    case "Asia":
                        BookManager.Instance.SetRoomAsia();
                        break;
                    case "NorthAmerica":
                        BookManager.Instance.SetRoomNorthAmerica();
                        break;
                    case "SouthAmerica":
                        BookManager.Instance.SetRoomSouthAmerica();
                        break;
                    case "Africa":
                        BookManager.Instance.SetRoomAfrica();
                        break;
                    case "Australia":
                        BookManager.Instance.SetRoomAustralia();
                        break;
                    default:
                        Debug.Log("Destination is wrongly set!");
                        break;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag.Equals("Player")){
            inPortal = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag.Equals("Player")){
            inPortal = false;
        }
    }    
}
