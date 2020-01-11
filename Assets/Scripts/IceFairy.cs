using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceFairy : MonoBehaviour
{
    public bool active = false;
    public GameObject[] spikeSpawnLocations;
    public GameObject spikeLocationParent;
    public GameObject icicle;
    public GameObject icicleStraight;
    public GameObject spike;
    public Vector3 spikePivotDifference;
    public float dropSpeed;
    private bool isDropping;
    public float spikeMoveDownTime;
    public float spikeDistance;
    private Vector3 originalSpikeLoc;
    private bool spikesDropping = false;
    private bool spikesRising = false;
    private Vector3 preRisePos;
    private bool rising;
    private bool risingOffscreen;
    private float prewaittime;
    private bool waiting;
    public bool hurtable = true;
    private float iFrames;
    private Sprite originalSprite;
    public Sprite WhiteSprite;
    private SpriteRenderer spriteRenderer;
    public float health = 1;
    public Slider healthUI;

    // Start is called before the first frame update

    public void Hurt(){
        if(hurtable){
            iFrames = 1.5f;
            health -= 0.05f;
        }
    }

    void Start()
    {
        originalSpikeLoc = spikeLocationParent.transform.position;
       spriteRenderer=gameObject.GetComponent<SpriteRenderer>();
       originalSprite = spriteRenderer.sprite;

       Fire();
       iFrames=3;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKeyDown("space"))
            Hurt();

        if(isDropping){
            transform.Translate(Vector3.down * dropSpeed * Time.fixedDeltaTime);
        }

        if(spikesDropping) {
            spikeLocationParent.transform.Translate(Vector3.down * Time.fixedDeltaTime);
            if(Vector3.Distance(spikeLocationParent.transform.position, originalSpikeLoc)>spikeDistance){
                spikesDropping=false;
                Drop();
            }
        }

        if(spikesRising) {
            spikeLocationParent.transform.Translate(Vector3.up * Time.fixedDeltaTime);
            if(Vector3.Distance(spikeLocationParent.transform.position, originalSpikeLoc)<0.1){
                spikesRising=false;
                RiseToShoot();
            }
        }

        if(rising){
            transform.Translate(Vector3.up * Time.fixedDeltaTime);
            if(Vector3.Distance(preRisePos, transform.position)>4){
                rising=false;
                Fire();
            }
        }

        if(risingOffscreen){
            transform.Translate(Vector3.up *20* Time.fixedDeltaTime);
            if(Vector3.Distance(preRisePos, transform.position)>30){
                risingOffscreen=false;
                DropSpikes();
            }

        }
        if(waiting){
            if(Time.time - prewaittime>2){
                waiting=false;
                GoOffscreen();
            }

        }

        if (iFrames > 0){
            iFrames -= Time.fixedDeltaTime;
            hurtable = false;
        } else {
            hurtable = true;
            ResetMaterial();
        }

        healthUI.value = health;
    }

    void ResetMaterial() {
        spriteRenderer.sprite = originalSprite;
    }

    public void Update(){
        if(iFrames > 0){
            FlashMaterial();
        }
    }

    void FlashMaterial() {
        float flashtime = 0.08333f;
        if(Time.time%flashtime < flashtime/2 ) {
            spriteRenderer.sprite = WhiteSprite;
        } else {
            spriteRenderer.sprite = originalSprite;
        }
    }

    public void Hit() {
        iFrames = 1;
    }

    public void Fire() {
        int n = Random.Range(3, 10);
        for(int i=0; i<n;i++) {
            var quart = Quaternion.AngleAxis(i*(360/n), Vector3.forward);
            GameObject.Instantiate(icicle, transform.position,quart);
        }
        WaitAfterShot();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "ground"){
            isDropping=false;
            RaiseSpikes();
            ThrowSingleIce();
        }
    }

    public void Drop() {
        isDropping = true;
        var quart = Quaternion.AngleAxis(-90, Vector3.forward);
        var g1 = GameObject.Instantiate(icicleStraight, transform.position + new Vector3(2, 10, 0), quart);
        var g2 = GameObject.Instantiate(icicleStraight, transform.position + new Vector3(1, 7, 0), quart);
        var g3 = GameObject.Instantiate(icicleStraight, transform.position + new Vector3(-1, 7, 0), quart);
        var g4 = GameObject.Instantiate(icicleStraight, transform.position + new Vector3(-2, 10, 0), quart);
        g1.transform.Rotate(new Vector3(0, 0, 20));
        g2.transform.Rotate(new Vector3(0, 0, 10));
        g3.transform.Rotate(new Vector3(0, 0, -10));
        g4.transform.Rotate(new Vector3(0, 0, -20));
        transform.SetParent(null);
    }

    public void Teleport() {
        int dropPosition = Random.Range(0, spikeSpawnLocations.Length);
        transform.position = spikeSpawnLocations[dropPosition].transform.position + spikePivotDifference;
        transform.SetParent(spikeLocationParent.transform);
    }

    public void DropSpikes(){
        spikesDropping = true;
        Teleport();
    }

    public void RaiseSpikes(){
        spikesRising = true;
    }

    public void RiseToShoot() {
        preRisePos=transform.position;
        rising=true;
    }

    public void GoOffscreen() {
        risingOffscreen=true;
    }

    public void WaitAfterShot() {
       prewaittime=Time.time;
       waiting = true;
    }

    public void ThrowSingleIce(){
            var ice = GameObject.Instantiate(icicleStraight, transform.position,Quaternion.identity);
            var player = Object.FindObjectOfType<Player>().gameObject;
            var playerPos = player.transform;
            ice.transform.LookAt(playerPos,Vector3.up);
            ice.transform.Rotate(new Vector3(0, -90, 0), Space.Self);
    }
}
