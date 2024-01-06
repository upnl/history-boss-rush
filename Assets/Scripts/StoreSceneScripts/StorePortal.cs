using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
                        SceneLoader.Instance.LoadScene("StoreScene");
                        break;
                    case "Europe":
                        BookManager.Instance.SetRoomEurope();
                        SceneLoader.Instance.LoadScene("StoreScene");
                        break;
                    case "Asia":
                        BookManager.Instance.SetRoomAsia();
                        SceneLoader.Instance.LoadScene("StoreScene");
                        break;
                    case "NorthAmerica":
                        BookManager.Instance.SetRoomNorthAmerica();
                        SceneLoader.Instance.LoadScene("StoreScene");
                        break;
                    case "SouthAmerica":
                        BookManager.Instance.SetRoomSouthAmerica();
                        SceneLoader.Instance.LoadScene("StoreScene");
                        break;
                    case "Africa":
                        BookManager.Instance.SetRoomAfrica();
                        SceneLoader.Instance.LoadScene("StoreScene");
                        break;
                    case "Australia":
                        BookManager.Instance.SetRoomAustralia();
                        SceneLoader.Instance.LoadScene("StoreScene");
                        break;
                    case "WorldMap":
                        SceneLoader.Instance.LoadScene("FieldScene");
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
