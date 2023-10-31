
using NUnit.Framework;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class EconomyTests {

    [Test]
    public void LoadEconomyDataFromAssets() {
        BuildingBalanceDescription balanceData = null;
        RessourceDescription ressourceData = null;

        balanceData = LoadBuildingBalanceDescription();
        ressourceData = LoadRessourceDescription();

        //Assert.IsTrue(balanceData != null && ressourceData != null);
        Assert.IsNotNull(balanceData);
        Assert.IsNotNull(ressourceData);
    }

    [Test]
    public void CreateEconomySystemInfoInstanz() {

        SetUpEconomyInsanz();


        Assert.AreEqual(true, EconemySystemInfo.Instanz.IsSet);
        Assert.AreEqual(1, EconemySystemInfo.Instanz.TickTimeInSeconds);
        Assert.AreEqual(1, EconemySystemInfo.Instanz.PlayerList.Count);
        Assert.IsNotNull(EconemySystemInfo.Instanz.RessourceDescription);
        Assert.IsNotNull(EconemySystemInfo.Instanz.GetBuildingProductionDescription(1), "Building with ID 1 does not exist");

    }


    [Test]
    public void RessourcesExists() {
        SetUpEconomyInsanz();

        List<Ressources> ressourceList = GetRessourceList();


        Assert.Greater(ressourceList.Count, 0);
    }

    [Test]
    public void CheckAddingRessourcesToPlayer() {

        SetUpEconomyInsanz();

        List<RessourcesValue> valueList = AddRessourcesToPlayer(1);

        foreach (RessourcesValue value in valueList) {

            RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(value.Ressources);

            if (value.Value > info.Cap && info.Type == RessourceTyp.valueType) {
                value.Value = info.Cap;

            }

            string errorMessage = value.Ressources.ToString() + ": " + value.Value.ToString() + " is not the same as ";
            errorMessage = errorMessage + EconemySystemInfo.Instanz.PlayerList[0].GetRessourcesValue(value.Ressources).Ressources + ": " + EconemySystemInfo.Instanz.PlayerList[0].GetRessourcesValue(value.Ressources).Value;

            Assert.AreEqual(value.Ressources, EconemySystemInfo.Instanz.PlayerList[0].GetRessourcesValue(value.Ressources).Ressources, errorMessage);
            Assert.AreEqual(value.Value, EconemySystemInfo.Instanz.PlayerList[0].GetRessourcesValue(value.Ressources).Value, errorMessage);

        }

    }

    [Test]
    public void CheckAddingHugeAmountsofRessourcesToPlayer() {
        SetUpEconomyInsanz();

        List<RessourcesValue> valueList = AddRessourcesToPlayer(10000);


        valueList = valueList.GroupBy(x => x.Ressources).Select(y => new RessourcesValue(y.Key, y.Sum(e => e.Value))).ToList();






        foreach (RessourcesValue value in valueList) {

            RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(value.Ressources);

            if (value.Value > info.Cap && info.Type == RessourceTyp.valueType) {
                value.Value = info.Cap;

            }

            string errorMessage = value.Ressources.ToString() + ": " + value.Value.ToString() + " is not the same as ";
            errorMessage = errorMessage + EconemySystemInfo.Instanz.PlayerList[0].GetRessourcesValue(value.Ressources).Ressources + ": " + EconemySystemInfo.Instanz.PlayerList[0].GetRessourcesValue(value.Ressources).Value;

            Assert.AreEqual(value.Ressources, EconemySystemInfo.Instanz.PlayerList[0].GetRessourcesValue(value.Ressources).Ressources, errorMessage);
            Assert.AreEqual(value.Value, EconemySystemInfo.Instanz.PlayerList[0].GetRessourcesValue(value.Ressources).Value, errorMessage);

        }
    }


    [Test]
    public void CheckAddingConsumptionRessourcesToPlayer() {
        SetUpEconomyInsanz();

        List<RessourcesValue> valueList = AddConsumerToPlayer(1);
        List<RessourcesValue> newValueList;

        newValueList = valueList.GroupBy(x => x.Ressources).Select(y => new RessourcesValue(y.Key, y.Sum(e => e.Value))).ToList();

        foreach (RessourcesValue value in newValueList) {

            string errorMessage = value.Ressources.ToString() + ": " + value.Value.ToString() + " is not the same as ";
            errorMessage = errorMessage + EconemySystemInfo.Instanz.PlayerList[0].GetTotalConsumption(value.Ressources).Ressources + ": " + EconemySystemInfo.Instanz.PlayerList[0].GetTotalConsumption(value.Ressources).Value;

            Assert.AreEqual(value.Ressources, EconemySystemInfo.Instanz.PlayerList[0].GetTotalConsumption(value.Ressources).Ressources, errorMessage);
            Assert.AreEqual(value.Value, EconemySystemInfo.Instanz.PlayerList[0].GetTotalConsumption(value.Ressources).Value, errorMessage);



        }
    }
    [Test]
    public void CheckAddingBulkConsumptionRessourcesToPlayer() {
        SetUpEconomyInsanz();

        List<RessourcesValue> valueList = AddConsumerToPlayer(10000);
        List<RessourcesValue> newValueList;

        newValueList = valueList.GroupBy(x => x.Ressources).Select(y => new RessourcesValue(y.Key, y.Sum(e => e.Value))).ToList();

        foreach (RessourcesValue value in newValueList) {

            string errorMessage = value.Ressources.ToString() + ": " + value.Value.ToString() + " is not the same as ";
            errorMessage = errorMessage + EconemySystemInfo.Instanz.PlayerList[0].GetTotalConsumption(value.Ressources).Ressources + ": " + EconemySystemInfo.Instanz.PlayerList[0].GetTotalConsumption(value.Ressources).Value;

            Assert.AreEqual(value.Ressources, EconemySystemInfo.Instanz.PlayerList[0].GetTotalConsumption(value.Ressources).Ressources, errorMessage);
            Assert.AreEqual(value.Value, EconemySystemInfo.Instanz.PlayerList[0].GetTotalConsumption(value.Ressources).Value, errorMessage);

        }
    }


    [Test]
    public void CheckEvenlyDistributionTimeout() {

        SetUpEconomyInsanz();

        AddRessourcesToPlayer(10000);
        AddConsumerToPlayer(10000);

        Stopwatch stopwatch = new Stopwatch();



        stopwatch.Start();

        bool check = false;

        Thread thread = new Thread(delegate () {
            EconemySystemInfo.Instanz.Distribution();
            check = true;
        }
        );

        thread.Start();

        while (check == false) {
            if (stopwatch.ElapsedMilliseconds > 1000) {
                thread.Abort();
                break;
            }
        }

        Assert.IsTrue(check, "Timeout");



    }


    [Test]
    public void CheckEvenlyDistributionLowRessources() {

        SetUpEconomyInsanz();
        List<Ressources> ressourcesList = GetRessourceList();


        List<RessourcesValue> addValues = AddRessourcesToPlayer(100);


        List<RessourcesValue> consumerValues = AddConsumerToPlayer(10000);


        List<RessourcesValue> sumAddValues = addValues.GroupBy(x => x.Ressources).Select(y => new RessourcesValue(y.Key, y.Sum(e => e.Value))).ToList();





        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();

        bool check = false;

        Thread thread = new Thread(delegate () {

            EconemySystemInfo.Instanz.Distribution();

            check = true;
        }
        );

        thread.Start();

        while (check == false) {
            if (stopwatch.ElapsedMilliseconds > 1000) {
                thread.Abort();
                break;
            }
        }

        Assert.IsTrue(check, "Timeout");



        foreach (Ressources r in ressourcesList) {

            Assert.LessOrEqual(EconemySystemInfo.Instanz.PlayerList[0].GetRessourcesValue(r).Value, 0);

        }

        List<RessourcesValue> sumConsumstored = consumerValues.GroupBy(x => x.Ressources).Select(y => new RessourcesValue(y.Key, y.Sum(e => e.Stored))).ToList();



        foreach (RessourcesValue value in sumConsumstored) {

            RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(value.Ressources);

            if (info.CanGoNegativ == true || info.Type == RessourceTyp.limitType || info.Type == RessourceTyp.distributionType) {

                continue;
            }

            RessourcesValue summedValue = new RessourcesValue(value.Ressources, sumAddValues.FirstOrDefault(x => x.Ressources == value.Ressources).Value);
            if (summedValue.Value > info.Cap) {
                summedValue.Value = info.Cap;
            }


            Assert.That(value.Value, Is.InRange(summedValue.Value - 2, summedValue.Value + 2));
        }
    }

    [Test]
    public void CheckEvenlyDistributionRessourcesMulipleTimes() {

        SetUpEconomyInsanz();

        List<RessourcesValue> addValues = AddRessourcesToPlayer(10000);
        List<RessourcesValue> consumerValues = AddConsumerToPlayer(1000);


        List<RessourcesValue> sumAddValues = addValues.GroupBy(x => x.Ressources).Select(y => new RessourcesValue(y.Key, y.Sum(e => e.Value))).ToList();



        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();

        bool check = false;

        Thread thread = new Thread(delegate () {
            EconemySystemInfo.Instanz.Distribution();
            EconemySystemInfo.Instanz.Distribution();
            EconemySystemInfo.Instanz.Distribution();
            EconemySystemInfo.Instanz.Distribution();
            EconemySystemInfo.Instanz.Distribution();

            check = true;
        }
        );

        thread.Start();

        while (check == false) {
            if (stopwatch.ElapsedMilliseconds > 5000) {
                thread.Abort();
                break;
            }
        }

        Assert.IsTrue(check, "Timeout");

        List<Ressources> ressourcesList = GetRessourceList();



        List<RessourcesValue> sumConsumstored = consumerValues.GroupBy(x => x.Ressources).Select(y => new RessourcesValue(y.Key, y.Sum(e => e.Stored))).ToList();



        foreach (RessourcesValue value in sumConsumstored) {

            RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(value.Ressources);

            if (info.CanGoNegativ == true || info.Type == RessourceTyp.limitType || info.Type == RessourceTyp.distributionType) {

                continue;
            }

            RessourcesValue summedValue = new RessourcesValue(value.Ressources, sumAddValues.FirstOrDefault(x => x.Ressources == value.Ressources).Value);
            if (summedValue.Value > info.Cap) {
                summedValue.Value = info.Cap;
            }

            Assert.That(value.Value, Is.InRange(summedValue.Value - 2 - EconemySystemInfo.Instanz.PlayerList[0].GetRessourcesValue(value.Ressources).Value, summedValue.Value + 2 - EconemySystemInfo.Instanz.PlayerList[0].GetRessourcesValue(value.Ressources).Value));
        }

    }
    [Test]
    public void CheckEvenlyDistributionLowRessourcesMulipleTimes() {

        SetUpEconomyInsanz();

        List<RessourcesValue> addValues = AddRessourcesToPlayer(1000);
        List<RessourcesValue> consumerValues = AddConsumerToPlayer(10000);


        List<RessourcesValue> sumAddValues = addValues.GroupBy(x => x.Ressources).Select(y => new RessourcesValue(y.Key, y.Sum(e => e.Value))).ToList();



        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();

        bool check = false;

        Thread thread = new Thread(delegate () {
            EconemySystemInfo.Instanz.Distribution();
            EconemySystemInfo.Instanz.Distribution();
            EconemySystemInfo.Instanz.Distribution();
            EconemySystemInfo.Instanz.Distribution();
            EconemySystemInfo.Instanz.Distribution();

            check = true;
        }
        );

        thread.Start();

        while (check == false) {
            if (stopwatch.ElapsedMilliseconds > 5000) {
                thread.Abort();
                break;
            }
        }

        Assert.IsTrue(check, "Timeout");

        List<Ressources> ressourcesList = GetRessourceList();



        List<RessourcesValue> sumConsumstored = consumerValues.GroupBy(x => x.Ressources).Select(y => new RessourcesValue(y.Key, y.Sum(e => e.Stored))).ToList();



        foreach (RessourcesValue value in sumConsumstored) {

            RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(value.Ressources);


            if (info.CanGoNegativ == true || info.Type == RessourceTyp.limitType || info.Type == RessourceTyp.distributionType) {

                continue;
            }

            RessourcesValue summedValue = new RessourcesValue(value.Ressources, sumAddValues.FirstOrDefault(x => x.Ressources == value.Ressources).Value);
            if (summedValue.Value > info.Cap) {
                summedValue.Value = info.Cap;
            }

            Assert.That(value.Value, Is.InRange(summedValue.Value - 2 - EconemySystemInfo.Instanz.PlayerList[0].GetRessourcesValue(value.Ressources).Value, summedValue.Value + 2 - EconemySystemInfo.Instanz.PlayerList[0].GetRessourcesValue(value.Ressources).Value));
        }

    }


    private List<RessourcesValue> AddConsumerToPlayer(int amount) {
        List<Ressources> ressourcesList = GetRessourceList();
        List<RessourcesValue> valueList = new List<RessourcesValue>();
        for (int i = 0; i < amount;) {

            foreach (Ressources ressources in ressourcesList) {
                valueList.Add(new RessourcesValue(ressources, Random.Range(0, 101)));
            }

            i = i + 1;
        }


        foreach (RessourcesValue value in valueList) {
            EconemySystemInfo.Instanz.PlayerList[0].AddConsumption(value);
        }




        return valueList;
    }

    private List<RessourcesValue> AddRessourcesToPlayer(int amount) {
        List<Ressources> ressourcesList = GetRessourceList();
        List<RessourcesValue> valueList = new List<RessourcesValue>();


        for (int i = 0; i < amount;) {

            foreach (Ressources ressources in ressourcesList) {

                RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(ressources);
                if (info.Type == RessourceTyp.distributionType) {
                    continue;
                }
                float valueToAdd = Random.Range(0, 101);


                valueList.Add(new RessourcesValue(ressources, valueToAdd));
            }

            i = i + 1;
        }


        foreach (RessourcesValue value in valueList) {
            RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(value.Ressources);
            if (info.Type == RessourceTyp.distributionType) {

            }
            else if (info.Type == RessourceTyp.limitType) {
                EconemySystemInfo.Instanz.PlayerList[0].AddRessourceCapLocked(value);

            }
            else {
                EconemySystemInfo.Instanz.PlayerList[0].AddingRessourceValueLocked(value);
            }

        }


        return valueList;
    }


    private List<Ressources> GetRessourceList() {
        List<Ressources> ressourceList = new List<Ressources>();
        foreach (Ressources ressources in EconemySystemInfo.Instanz.RessourceDescription.Keys) {
            ressourceList.Add(ressources);
        }
        return ressourceList;
    }

    private BuildingBalanceDescription LoadBuildingBalanceDescription() {
        BuildingBalanceDescription balanceData = AssetDatabase.LoadAssetAtPath<BuildingBalanceDescription>("Assets/02_Scripts/Runtime/Ecconemy/Buildings/BuildingBalanceDescriptionData.asset");
        return balanceData;
    }

    private RessourceDescription LoadRessourceDescription() {
        RessourceDescription ressourceData = AssetDatabase.LoadAssetAtPath<RessourceDescription>("Assets/02_Scripts/Runtime/Ecconemy/Ressources/RessourceDescriptionData.asset");
        return ressourceData;
    }

    private void SetUpEconomyInsanz() {
        BuildingBalanceDescription balanceData = LoadBuildingBalanceDescription();
        RessourceDescription ressourceData = LoadRessourceDescription();
        float tickTimeInSeconds = 1;
        PlayerBilanzInfo playerBilanzInfo = new PlayerBilanzInfo();
        List<PlayerBilanzInfo> players = new List<PlayerBilanzInfo>();

        players.Add(playerBilanzInfo);


        EconemySystemInfo instanz = EconemySystemInfo.Instanz;

        instanz.SetEconemySystemInfo(tickTimeInSeconds, ressourceData, balanceData, players, false);

    }

}
