using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour {

    private bool onksKameraKaytossa;
    private WebCamTexture kamera;
    private Texture normiTausta;

    public RawImage raakaKuva;
    public AspectRatioFitter fitterino;



    // Start is called before the first frame update
    void Start() {

        normiTausta = raakaKuva.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0) {
            Debug.Log("ei löytynyt kameraa");
        } else {
            Debug.Log("kamera löytyi!");
        }

        for (int i = 0; i < devices.Length; i++) {
            if (!devices[i].isFrontFacing) {
                kamera = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if(kamera == null) {
            Debug.Log("kameraa ei löytynyt sittenkään. Suljetaan skripti");
            return;
        }

        //noniin nyt meillä on kamera joka ei ole null.

        kamera.Play();

        raakaKuva.texture = kamera;

        onksKameraKaytossa = true; 

        
        
    }

    // Update is called once per frame
    void Update() {
        
        if(!onksKameraKaytossa) {
            return;
        }

        float ratio = (float)kamera.width / (float)kamera.height;
        fitterino.aspectRatio = ratio;

        float scaleY = kamera.videoVerticallyMirrored ? -1f : 1f;
        raakaKuva.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -kamera.videoRotationAngle;

        raakaKuva.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

    }
}
