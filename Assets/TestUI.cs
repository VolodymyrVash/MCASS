using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Следи за библиотеками, у тебя отсуствовали важніе для нас , єто System; UnityEngine.UI; NUnit.Framework;

public class TestSuite : UITest
{
    //Перезагрузил тебе Test, ты его менял, а этого пока не знаешь что делаешь не стоит )
    //В новой версии UIAutomation мі пишем не UISetUp а просто SetUp, єто связано с тем что фреймворк перекочевал на встроенный NUnit Unity
    //[SetUp] //Preset to start the scene. //SetUP пока не трогаем
    //public IEnumerable Init()
    //{
    //    yield return LoadSceneByPath("Assets/Game/Scene/ModeSelector.unity");
    //}

    [Test]//check that scene was loaded
    public IEnumerable CheckScena()
    {
        yield return LoadScene("MainMenu");
        var pause = new WaitForSeconds(0.1f);
        yield return WaitFor(new ObjectAppeared("Canvas"));
        yield return null;
    }

    private static int GetResult(string opName, int first, int second)
    {
        switch (opName)
        {
            case "minus":
                return first - second;
            case "plus":
                return first + second;
            case "divide":
                return first / second;
            case "multiply":
                return first * second;
            default:
                throw new ArgumentException("Unknown case");
        }
    }

    //TODO Попробуй автоматизировать пару кнопок. Поставь себе Resharper, архив с ним я тебе скину

    //Будет не Test а просто Test
    [Test] //Checking the presence of a sign // [***] Єто атрибут
    public IEnumerable PlusCheck()
    {
        yield return LoadScene("Assets/Game/Scene/ModeSelector.unity"); //Добавил эту строку для загрузки сцены. Читай Readme.txt в папке с фреймом
        var pause = new WaitForSeconds(0.1f);
        yield return WaitFor(new ObjectAppeared("Canvas"));
        yield return Press("AdditionButton");

        var opName = GameObject.FindGameObjectWithTag("Operation").GetComponent<Image>().sprite.name;
        Assert.AreEqual(opName, "plus");
        yield return null;
    }

    [Test] //Checking the health of the logic
    public IEnumerable LogicWithPlus()
    {
        yield return WaitFor(new ObjectAppeared("Canvas"));
        yield return Press("AdditionButton");

        yield return null;
        var firstStr = GameObject.FindGameObjectWithTag("FirstVal").GetComponent<Text>().text;
        var secondStr = GameObject.FindGameObjectWithTag("SecondVal").GetComponent<Text>().text;
        var opName = GameObject.FindGameObjectWithTag("Operation").GetComponent<Image>().sprite.name;


        var first = int.Parse(firstStr);
        var second = int.Parse(secondStr);

        var res = GetResult(opName, first, second);

        var button1 = GameObject.FindGameObjectWithTag("0");
        var button2 = GameObject.FindGameObjectWithTag("1");
        var button3 = GameObject.FindGameObjectWithTag("2");
        var button4 = GameObject.FindGameObjectWithTag("3");

        var ans1 = int.Parse(button1.GetComponentInChildren<Text>().text);
        if (ans1 == res)
            yield return Press(button1.gameObject);

        var ans2 = int.Parse(button2.GetComponentInChildren<Text>().text);
        if (ans2 == res)
            yield return Press(button2.gameObject);

        var ans3 = int.Parse(button3.GetComponentInChildren<Text>().text);
        if (ans3 == res)
            yield return Press(button3.gameObject);

        var ans4 = int.Parse(button4.GetComponentInChildren<Text>().text);
        if (ans4 == res)
            yield return Press(button4.gameObject);

        yield return AssertLabel("ScoreImage/ScoreText", "1");
    }
    [Test] //Playability check
    public IEnumerable PlusGameBot()
    {
        yield return null;
        var pause = new WaitForSeconds(0.1f);
        yield return WaitFor(new ObjectAppeared("Canvas"));
        yield return Press("AdditionButton");

        var ScoreFind = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>().text;
        int Score = int.Parse(ScoreFind);

        do
        {

            ScoreFind = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>().text;
            Score = int.Parse(ScoreFind);

            yield return WaitFor(new ObjectAppeared("Canvas"));

            var firstStr = GameObject.FindGameObjectWithTag("FirstVal").GetComponent<Text>().text;
            var secondStr = GameObject.FindGameObjectWithTag("SecondVal").GetComponent<Text>().text;
            var opName = GameObject.FindGameObjectWithTag("Operation").GetComponent<Image>().sprite.name;

            var first = int.Parse(firstStr);
            var second = int.Parse(secondStr);

            var res = GetResult(opName, first, second);

            var button = GameObject.FindGameObjectWithTag("0");
            var button2 = GameObject.FindGameObjectWithTag("1");
            var button3 = GameObject.FindGameObjectWithTag("2");
            var button4 = GameObject.FindGameObjectWithTag("3");

            var ans1 = int.Parse(button.GetComponentInChildren<Text>().text);
            if (ans1 == res)
                yield return Press(button.gameObject);

            var ans2 = int.Parse(button2.GetComponentInChildren<Text>().text);
            if (ans2 == res)
                yield return Press(button2.gameObject);

            var ans3 = int.Parse(button3.GetComponentInChildren<Text>().text);
            if (ans3 == res)
                yield return Press(button3.gameObject);

            var ans4 = int.Parse(button4.GetComponentInChildren<Text>().text);
            if (ans4 == res)
                yield return Press(button4.gameObject);
            yield return null;
        } while (Score < 99);

        if (Score == 99)
        {
            yield return AssertLabel("ScoreImage/ScoreText", "100");
        }
        else
        {
            throw new ArgumentException("Invalid Score");
        }
    }
} // Когда копируешь чейто код следи за скобочками, они теряються. Поставил скобочку