using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : BaseController<AudioController>
{
    public AudioSource MusicSource;
    public AudioSource AudioSource;
    
    public AudioClip Music;
    public AudioClip ButtonSound;
    public AudioClip LootSound;
    public AudioClip HoneySound;
    public AudioClip CoinSound;
    public AudioClip SuspenseSound;
    public AudioClip BearGrowlSound;
    public AudioClip BearEatSound;
    public AudioClip BuildSound;
    public AudioClip UpgradeSound;
    


    void Start()
    {
        PlayerController.Instance.OnLoot += OnPlayerLoot;
        ScenarioController.Instance.OnScenarioAnimation += OnScenarioAnimation;
        FarmController.Instance.OnBuild += OnBuild;
    }

    private void OnBuild(BuildElement element) =>
        StartCoroutine(PlayAnimationSound("player", "build"));

    private void OnScenarioAnimation(string actor, string status) =>
        StartCoroutine(PlayAnimationSound(actor, status));

    private IEnumerator PlayAnimationSound(string  actor, string status)
    {
        switch (actor)
        {
            case "bear":
                switch (status)
                {
                    case "walk":
                        this.Play(SuspenseSound);
                        yield return new WaitForSeconds(4.0f);
                        this.Play(BearGrowlSound);
                        break;
                    case "eat":
                        this.Play(BearEatSound);
                        break;
                }
                break;
            case "player":
                switch (status)
                {
                    case "build":
                        for (int i = 0; i < 3; i++)
                        {
                            this.Play(BuildSound);
                            yield return new WaitForSeconds(0.33f);
                        }
                        break;
                }
                break;
            case "beehive":
                switch (status)
                {
                    case "upgrade":
                        this.Play(UpgradeSound);
                        break;
                }
                break;
        }
    }


    private void OnPlayerLoot(string resource, float quantity, IInteractable target)
    {
        if (resource == "money")
            this.Play(CoinSound);
        else if (resource == "honey")
            this.Play(HoneySound);
        else            
            this.Play(LootSound);
    }

    void Update()
    {
       // if(!MusicSource.isPlaying)
        //    MusicSource.PlayOneShot(Music);
            
    }

    private void Play(AudioClip clip)
    {
        AudioSource.PlayOneShot(clip);
    }
}
