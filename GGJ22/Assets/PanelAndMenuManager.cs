using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAndMenuManager : MonoBehaviour {

    public static PanelAndMenuManager instance;
    public GameObject popupikkuna;
    public Texture otettuKuva;

    // Start is called before the first frame update
    void Awake() {

        instance = this;
        
    }

    public void OpenPopUp(Texture kuva) {

        popupikkuna.SetActive(true);
        //otettuKuva.sourceImage = kuva; 
    }

    public void ClosePopUp() {

        popupikkuna.SetActive(false);

    }
 
    
}
