using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;


public class CheckButton : UITest
{
    [UnityTest] //check that scene was loaded
    public IEnumerator CheckScena()
    {
        yield return LoadScene("MainMenu");
        var pause = new WaitForSeconds(0.1f);
        yield return WaitFor(new ObjectAppeared("Canvas"));
        var button1 = GameObject.Find("PlayButton");
        var result1 = button1.GetComponent<Image>().sprite.name;
        Assert.AreEqual(result1, "play");

        var button2 = GameObject.Find("QuitButton");
        var result2 = button2.GetComponent<Image>().sprite.name;
        Assert.AreEqual(result2, "quit");

        var button3 = GameObject.Find("SettingButton");
        var result3 = button3.GetComponent<Image>().sprite.name;
        Assert.AreEqual(result3, "setting");
    }

    [UnityTest]
    public IEnumerator checkSoundButton()
    {
        yield return LoadScene("MainMenu");
        var pause = new WaitForSeconds(1f);
        yield return WaitFor(new ObjectAppeared("Canvas"));

        var button3 = GameObject.Find("SettingButton");
        yield return Press("SettingButton");
        yield return pause;
        
        var button4 = GameObject.Find("Soundbutton");
        yield return pause;
        try
        {
            var result4 = button4.GetComponent<Image>().sprite.name;
            Assert.AreEqual(result4, "soundon");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }


    }




}
