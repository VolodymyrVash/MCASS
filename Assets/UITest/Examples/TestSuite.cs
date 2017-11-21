using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Gurock.TestRail;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TestSuite : UITest
{
    public static bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                    }
                }
            }
        }
        return isOk;
    }

#if UNITY_EDITOR
    public static string deviceModel = "Unity";
#elif UNITY_ANDROID
    public static string deviceModel = SystemInfo.deviceModel;
#elif UNITY_IOS
    public static string deviceModel = SystemInfo.deviceModel;
#endif

    public static string testRunName;

    public static String caseID;
    public static String testrunID;

    

    public class TestRun
    {
        public int id { get; set; }
        public int suite_id { get; set; }
        public string name { get; set; }
        public object description { get; set; }
        public object milestone_id { get; set; }
        public object assignedto_id { get; set; }
        public bool include_all { get; set; }
        public bool is_completed { get; set; }
        public object completed_on { get; set; }
        public object config { get; set; }
        public List<object> config_ids { get; set; }
        public int passed_count { get; set; }
        public int blocked_count { get; set; }
        public int untested_count { get; set; }
        public int retest_count { get; set; }
        public int failed_count { get; set; }
        public int custom_status1_count { get; set; }
        public int custom_status2_count { get; set; }
        public int custom_status3_count { get; set; }
        public int custom_status4_count { get; set; }
        public int custom_status5_count { get; set; }
        public int custom_status6_count { get; set; }
        public int custom_status7_count { get; set; }
        public int project_id { get; set; }
        public object plan_id { get; set; }
        public int created_on { get; set; }
        public int created_by { get; set; }
        public string url { get; set; }
    }

    public class RequestsToTestRail
    {

        public static void SetUp()
        {
            //Название устройства в имя тестрана
            //Название билда в имя тестрана

            //Создание тестрана
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            APIClient client = new APIClient("https://qa.room8studio.com/");
            client.User = "e.kalina@room8studio.com";
            client.Password = "TOpsexIds.wd1TuNECt7-v.Mm.hsuW8kJTCuAV2iX";

            var data = new Dictionary<string, object>
            {
                { "name", "Automation_Test_Run_DATE_"+ deviceModel },
                { "suite_id", 369 },
            };

            Debug.Log(data);
            var jContainer =  client.SendPost("add_run/2", data);

            var response = client.SendGet("get_runs/2&is_completed=0").ToString();

            var array = JsonConvert.DeserializeObject<List<TestRun>>(response);

            var InArray = array[0];
            testRunName = InArray.name +"_"+ deviceModel;
        }

        //Выбрать кейс выставить ему пасс
        public static void TestPass()
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            APIClient client = new APIClient("https://qa.room8studio.com/");
            client.User = "e.kalina@room8studio.com";
            client.Password = "TOpsexIds.wd1TuNECt7-v.Mm.hsuW8kJTCuAV2iX"; 

            var response = client.SendGet("get_runs/2&is_completed=0").ToString();

            var array = JsonConvert.DeserializeObject<List<TestRun>>(response);

            var InArray = array[0];
            var id = InArray.id;

            var data1 = new Dictionary<string, object>
                {
                    {"status_id", 1},
                    {"comment", "Cool"},
                };
                 
            JContainer jContainer1 = (JContainer)client.SendPost("add_result_for_case/" + id + "/" + caseID, data1);
        }

        //Выбрать кейс выставить ему фейл
        public static void TestFail()
        {
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            APIClient client = new APIClient("https://qa.room8studio.com/");
            client.User = "e.kalina@room8studio.com";
            client.Password = "TOpsexIds.wd1TuNECt7-v.Mm.hsuW8kJTCuAV2iX";

            var response = client.SendGet("get_runs/2&is_completed=0").ToString();
            var array = JsonConvert.DeserializeObject<List<TestRun>>(response);

            var InArray = array[0];
            var id = InArray.id;

            var data1 = new Dictionary<string, object>
                {
                    {"status_id", 5},
                    {"comment", "NotCool"},
                };

            JContainer jContainer1 = (JContainer)client.SendPost("add_result_for_case/" + id + "/" + caseID, data1);
        }

        //public static void TestRailCheck()
        //{
        //    ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        //    APIClient client = new APIClient("https://qa.room8studio.com/");
        //    client.User = "e.kalina@room8studio.com";
        //    client.Password = "TOpsexIds.wd1TuNECt7-v.Mm.hsuW8kJTCuAV2iX";

        //    var response = client.SendGet("get_runs/2&is_completed=0").ToString();

        //    var array = JsonConvert.DeserializeObject<List<TestRun>>(response);

        //    var InArray = array[0];
        //    testRunName = InArray.name;
        //}
    }

    [UISetUp]
    public IEnumerable Init()
    {
#if UNITY_EDITOR
        yield return LoadSceneByPath("Assets/Game/Scene/ModeSelector.unity");
#elif UNITY_ANDROID
        yield return LoadScene("ModeSelector");
#elif UNITY_IOS
        yield return LoadScene("ModeSelector");
#endif
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



    [UITest]
    public IEnumerable ApiCheck()
    {
        caseID = "46760";

        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

        RequestsToTestRail.SetUp();

        if (testRunName == "Automation Test Run 20.11")
        {
            RequestsToTestRail.TestPass();
        }
        else
        {
            RequestsToTestRail.TestFail();
        }

        yield return null;
    }

    [UITest]
    public IEnumerable PlusCheck()
    {
        caseID = "46761";
        yield return WaitFor(new ObjectAppeared("Canvas"));
        yield return Press("AdditionButton");


        var opName = GameObject.FindGameObjectWithTag("Operation").GetComponent<Image>().sprite.name;

        if (opName == "plus")
        {
            RequestsToTestRail.TestPass();
        }
        else
        {
            RequestsToTestRail.TestFail();
        }

        yield return null;
    }

    [UITest]
    public IEnumerable MinusCheck()
    {
        caseID = "46762";
        yield return WaitFor(new ObjectAppeared("Canvas"));
        yield return Press("SubtractionButton");


        var opName = GameObject.FindGameObjectWithTag("Operation").GetComponent<Image>().sprite.name;

        if (opName == "minus")
        {
            RequestsToTestRail.TestPass();
        }
        else
        {
            RequestsToTestRail.TestFail();
        }

        yield return null;
    }

    [UITest]
    public IEnumerable MultiplicationCheck()
    {
        caseID = "46763";
        yield return WaitFor(new ObjectAppeared("Canvas"));
        yield return Press("MultiplicationButton");


        var opName = GameObject.FindGameObjectWithTag("Operation").GetComponent<Image>().sprite.name;

        if (opName == "multiply")
        {
            RequestsToTestRail.TestPass();
        }
        else
        {
            RequestsToTestRail.TestFail();
        }

        yield return null;
    }

    [UITest]
    public IEnumerable DivaderCheck()
    {
        caseID = "46764";
        yield return WaitFor(new ObjectAppeared("Canvas"));
        yield return Press("DivisionButton");


        var opName = GameObject.FindGameObjectWithTag("Operation").GetComponent<Image>().sprite.name;

        if (opName == "divide")
        {
            RequestsToTestRail.TestPass();
        }
        else
        {
            RequestsToTestRail.TestFail();
        }

        yield return null;
    }

    [UITest]
    public IEnumerable LogicWithPlus()
    {
        caseID = "46765";

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
        var ScoreFind = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>().text;
        int Score = int.Parse(ScoreFind);

        if (Score == 1)
        {
            RequestsToTestRail.TestPass();
        }
        else
        {
            RequestsToTestRail.TestFail();
        }
    }

    [UITest]
    public IEnumerable LogicWithMinus()
    {
        caseID = "46766";

        yield return WaitFor(new ObjectAppeared("Canvas"));
        yield return Press("SubtractionButton");
        yield return null;
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

        var ScoreFind = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>().text;
        int Score = int.Parse(ScoreFind);

        if (Score == 2)
        {
            RequestsToTestRail.TestPass();
        }
        else
        {
            RequestsToTestRail.TestFail();
        }
    }

    [UITest]
    public IEnumerable LogicWithMultiply()
    {
        caseID = "46767";

        yield return WaitFor(new ObjectAppeared("Canvas"));
        yield return Press("MultiplicationButton");
        yield return null;
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

        var ScoreFind = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>().text;
        int Score = int.Parse(ScoreFind);

        if (Score == 3)
        {
            RequestsToTestRail.TestPass();
        }
        else
        {
            RequestsToTestRail.TestFail();
        }
    }

    [UITest]
    public IEnumerable LogicWithDivision()
    {
        caseID = "46768";

        yield return WaitFor(new ObjectAppeared("Canvas"));
        yield return Press("DivisionButton");
        yield return null;
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

        var ScoreFind = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>().text;
        int Score = int.Parse(ScoreFind);

        if (Score == 4)
        {
            RequestsToTestRail.TestPass();
        }
        else
        {
            RequestsToTestRail.TestFail();
        }
    }

    [UITest]
    public IEnumerable LogicWithMix()
    {
        caseID = "46769";

        yield return WaitFor(new ObjectAppeared("Canvas"));
        yield return Press("DivisionButton");
        yield return null;
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

        var ScoreFind = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>().text;
        int Score = int.Parse(ScoreFind);

        if (Score == 5)
        {
            RequestsToTestRail.TestPass();
        }
        else
        {
            RequestsToTestRail.TestFail();
        }
    }

    [UITest]
    public IEnumerable PlusGameBot()
    {
        caseID = "46770";

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
            RequestsToTestRail.TestPass();
        }
        else
        {
            RequestsToTestRail.TestFail();
        }
    }

    [UITest]
    public IEnumerable MinusGameBot()
    {
        caseID = "46771";

        yield return null;
        var pause = new WaitForSeconds(0.1f);
        yield return WaitFor(new ObjectAppeared("Canvas"));
        yield return Press("SubtractionButton");

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

        } while (Score < 199);

        if (Score == 199)
        {
            RequestsToTestRail.TestPass();
        }
        else
        {
            RequestsToTestRail.TestFail();
        }
    }

    [UITest]
    public IEnumerable MultiGameBot()
    {
        caseID = "46772";

        yield return null;
        var pause = new WaitForSeconds(0.1f);
        yield return WaitFor(new ObjectAppeared("Canvas"));
        yield return Press("MultiplicationButton");

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

        } while (Score < 299);

        if (Score == 299)
        {
            RequestsToTestRail.TestPass();
        }
        else
        {
            RequestsToTestRail.TestFail();
        }
    }

    [UITest]
    public IEnumerable DivideGameBot()
    {
        caseID = "46773";

        yield return null;
        var pause = new WaitForSeconds(0.1f);
        yield return WaitFor(new ObjectAppeared("Canvas"));
        yield return Press("DivisionButton");

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

        } while (Score < 399);

        if (Score == 399)
        {
            RequestsToTestRail.TestPass();
        }
        else
        {
            RequestsToTestRail.TestFail();
        }
    }

    [UITest]
    public IEnumerable MixGameBot()
    {
        caseID = "46774";

        yield return null;
        var pause = new WaitForSeconds(0.1f);
        yield return WaitFor(new ObjectAppeared("Canvas"));
        yield return Press("MixButton");

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

        } while (Score < 499);

        if (Score == 499)
        {
            RequestsToTestRail.TestPass();
        }
        else
        {
            RequestsToTestRail.TestFail();
        }
    }
}