using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Saver_Loader : MonoBehaviour {

    public static Saver_Loader instance;
    private string imagesFolderPath;
    private string audioFolderPath;

    // Start is called before the first frame update
    void Start() {

        Debug.Log("Kuvat tallennetaan tänne: " +Application.persistentDataPath);

        imagesFolderPath = Application.persistentDataPath;
        //image.GetComponent<SpriteRenderer>().sprite = last_screenshot_save;


        string testi = "testi1"; 
        //tsekataanOnkoKuvaOlemassa
        if (ImageExists(testi)) {
            Debug.Log("Kuva " + testi + " löydettiin!");
        } else {
            Debug.Log("Kuvaa " + testi + " ei löydetty tiedostosta :( ");
        }
       

    }

    void Awake() {
        instance = this;
    }



    //tsekkaa onko pathia olemassa.  
    public bool ImageExists(string path) {

        if (File.Exists(imagesFolderPath + "/" + path + ".jpg") || File.Exists(imagesFolderPath + "/" + path + ".png")) {
            return true; 
        } else {
            return false;
        }
    }


 


    // Lataa PNG tai JPG filun annetusta FilePathista. Returnaa sen kuvan. 
    public Sprite LoadSprite(string path) {
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path)) {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }



}
