﻿using ClockStone;
using MelonLoader;
using RMMBY.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace RMMBY.NeonLevelLoader
{
    internal class LevelSetup
    {
        public static UnityEngine.Object[] cardObjects;
        public static ActorData actorData;

        public static void Setup()
        {
            PlayerTeleport playerstart = GameObject.Find("Teleport_START").AddComponent<PlayerTeleport>();
            playerstart.id = "START";
            playerstart.grantCardsOnTeleport = new PlayerCardData[0];

            if (cardObjects == null)
                cardObjects = Resources.FindObjectsOfTypeAll(typeof(PlayerCardData));

            if (actorData == null)
                actorData = Resources.FindObjectsOfTypeAll(typeof(ActorData))[0] as ActorData;

            List<GameObject> spawnables = ObjectFinders.FindAllObjectsStartsWithName("Spawn_").ToList<GameObject>();
            List<GameObject> cards = ObjectFinders.FindAllObjectsContainingNameInList("_Card_", spawnables);
            List<GameObject> objects = ObjectFinders.FindAllObjectsContainingNameInList("_Object_", spawnables);

            CardSetup(cards.ToArray());

            EnemyWave wave = GameObject.Find("Enemies").AddComponent<EnemyWave>();
            wave.gameObject.AddComponent<EnemyEncounter>();

            ObjectSetup(objects.ToArray());

            MusicSetup(playerstart.gameObject);
        }

        public static void MusicSetup(GameObject tpStart)
        {
            if (!(tpStart.GetComponent<AudioSource>().clip != null))
            {
                GameData gd = Singleton<Game>.Instance.GetGameData();
                LevelData customLevel = gd.campaigns[2].missionData[0].levels[0];   
                Singleton<Game>.Instance.GetGameData().campaigns[2].missionData[0].levels[0].music = customLevel.music;
                return;
            }

            if (tpStart.GetComponent<AudioSource>().clip != null)
            {
                AudioController audioController = (AudioController)GameObject.Find("Audio").GetComponent<Audio>().GetType().GetField("_masterAudioController", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(GameObject.Find("Audio").GetComponent<Audio>());
                Dictionary<string, AudioItem> asi = (Dictionary<string, AudioItem>)audioController.GetType().GetField("_audioItems", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(audioController);

                tpStart.GetComponent<AudioSource>().playOnAwake = false;
                tpStart.GetComponent<AudioSource>().volume = 0;
                tpStart.GetComponent<AudioSource>().Stop();

                if (asi.ContainsKey(tpStart.GetComponent<AudioSource>().clip.name))
                {
                    asi[tpStart.GetComponent<AudioSource>().clip.name].subItems[0].Clip = tpStart.GetComponent<AudioSource>().clip;
                    Singleton<Game>.Instance.GetGameData().campaigns[2].missionData[0].levels[0].music = tpStart.GetComponent<AudioSource>().clip.name;

                    GameData gda = Singleton<Game>.Instance.GetGameData();

                    GameObject.Find("Audio").GetComponent<Audio>().PlayMusic(tpStart.GetComponent<AudioSource>().clip.name);
                    return;
                }

                AudioItem ai = new AudioItem();
                ai.Name = tpStart.GetComponent<AudioSource>().clip.name;
                ai.Loop = AudioItem.LoopMode.PlaySequenceAndLoopLast;
                AudioSubItem sub = new AudioSubItem();
                sub.Clip = tpStart.GetComponent<AudioSource>().clip;
                sub.SubItemType = AudioSubItemType.Clip;
                sub.Probability = 1;
                sub.FadeIn = 0.1f;
                sub.FadeOut = 0.1f;
                sub.ItemModeAudioID = "";

                sub.GetType().GetField("_item", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(sub, ai);

                ai.subItems = new AudioSubItem[1];
                ai.subItems[0] = sub;
                AudioCategory audioCategory = (AudioCategory)asi["MUSIC_GAMEPLAY_OLDCITY"].GetType().GetField("_category", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(asi["MUSIC_GAMEPLAY_OLDCITY"]);

                ai.GetType().GetField("_category", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(ai, audioCategory);

                asi.Add(tpStart.GetComponent<AudioSource>().clip.name, ai);

                Singleton<Game>.Instance.GetGameData().campaigns[2].missionData[0].levels[0].music = ai.Name;

                GameObject.Find("Audio").GetComponent<Audio>().PlayMusic(ai.Name);
            }
            else
            {
                GameData gd = Singleton<Game>.Instance.GetGameData();

                LevelData customLevel = gd.campaigns[2].missionData[0].levels[0];
                customLevel.music = "MUSIC_GAMEPLAY_GLASSOCEAN";
            }
        }

        public static void EnemySetup(GameObject[] enemies)
        {
            foreach (GameObject enemy in enemies)
            {
                Enemy.Type enemytype = Enemy.Type.balloon;

                string type = enemy.name.Replace("Spawn_Enemy_", "");

                switch (type)
                {
                    case "balloon":
                        enemytype = Enemy.Type.balloon;
                        break;
                    case "barnacle":
                        enemytype = Enemy.Type.barnacle;
                        break;
                    case "boxer":
                        enemytype = Enemy.Type.boxer;
                        break;
                    case "demonBall":
                        enemytype = Enemy.Type.demonBall;
                        break;
                    case "forcefield":
                        enemytype = Enemy.Type.forcefield;
                        break;
                    case "frog":
                        enemytype = Enemy.Type.frog;
                        break;
                    case "guardian":
                        enemytype = Enemy.Type.guardian;
                        break;
                    case "jock":
                        enemytype = Enemy.Type.jock;
                        break;
                    case "jumper":
                        enemytype = Enemy.Type.jumper;
                        break;
                    case "mimic":
                        enemytype = Enemy.Type.mimic;
                        break;
                    case "ringer":
                        enemytype = Enemy.Type.ringer;
                        break;
                    case "roller":
                        enemytype = Enemy.Type.roller;
                        break;
                    case "shocker":
                        enemytype = Enemy.Type.shocker;
                        break;
                    case "shockerAndForcefield":
                        enemytype = Enemy.Type.shockerAndForcefield;
                        break;
                    case "tripwire":
                        enemytype = Enemy.Type.tripwire;
                        break;
                    case "ufo":
                        enemytype = Enemy.Type.ufo;
                        break;
                }

                EnemySpawner spawner = enemy.AddComponent<EnemySpawner>();
                spawner.forcefieldRadiusOverride = 15;
                spawner.isStatic = true;
                spawner.followTransformOnSpawn = enemy.transform;
                spawner.spawnedByBossWeaponWave = EnemySpawner.BossWeaponWave.None;
                spawner.tripwireType = EnemyTripwire.TripwireType.Standard;
                spawner.validTypes = new Enemy.Type[1] { enemytype };
                spawner.waypointSetID = "DEFAULT";

                spawner.enabled = true;
            }
        }

        public static void CardSetup(GameObject[] cards)
        {
            foreach (GameObject card in cards)
            {
                string cardtype = card.name;

                if (cardtype.Contains("Machinegun")) cardtype = "Card_Weapon_Machinegun";
                else if (cardtype.Contains("Pistol")) cardtype = "Card_Weapon_Pistol";
                else if (cardtype.Contains("Rifle")) cardtype = "Card_Weapon_Rifle";
                else if (cardtype.Contains("RocketLauncher")) cardtype = "Card_Weapon_RocketLauncher";
                else if (cardtype.Contains("Shotgun")) cardtype = "Card_Weapon_Shotgun";
                else if (cardtype.Contains("UZI")) cardtype = "Card_Weapon_UZI";
                else if (cardtype.Contains("Katana")) cardtype = "Card_Sidearm_Katana";
                else if (cardtype.Contains("Katana_Miracle")) cardtype = "Card_Sidearm_Katana_Miracle";
                else if (cardtype.Contains("Health")) cardtype = "Card_Item_Health";
                else if (cardtype.Contains("Ammo")) cardtype = "Card_Item_Ammo";
                else if (cardtype.Contains("Memory")) cardtype = "Card_Item_Memory";
                else if (cardtype.Contains("Rapture")) cardtype = "Card_Ability_Rapture";

                CardPickupSpawner spawner = card.GetComponent<CardPickupSpawner>();
                spawner.alwaysSpawnThisTutorialCard = true;

                foreach (UnityEngine.Object obj in cardObjects)
                {
                    if (obj.name == cardtype)
                    {
                        spawner.card = obj as PlayerCardData;
                        break;
                    }
                }

                spawner.offsetHeightFromGround = true;
                spawner.overrideStartingAmmo = -1;
                spawner.spawnOnStart = true;
                string[] array2 = card.name.Split(new char[] { '/' });
                if (array2.Length == 1)
                {
                    spawner.vendingStock = 1;
                }
                else
                {
                    spawner.vendingStock = int.Parse(array2[1]);
                }
            }
        }

        public static void UISetup()
        {
            GameObject.Find("Level Name").GetComponent<TMP_Text>().text = Melon<Plugin>.Instance.currentLevel.Title;
            GameObject.Find("Level Title").GetComponent<TMP_Text>().text = Melon<Plugin>.Instance.currentLevel.Title;
            GameObject.Find("Level Environment").GetComponent<TMP_Text>().text = GetDistrictName(Melon<Plugin>.Instance.currentLevel.EnvironmentType);

            try
            {
                GameObject.Find("Leaderboards").SetActive(false);
            }
            catch { }
        }

        public static string GetDistrictName(int type)
        {
            string result = "";

            switch (type)
            {
                case 0:
                    result = "Sheol";
                    break;
                case 1:
                    result = "Sheol";
                    break;
                case 2:
                    result = "Sheol";
                    break;
                case 3:
                    result = "Sheol";
                    break;
                case 4:
                    result = "???";
                    break;
                case 5:
                    result = "Glass Port";
                    break;
                case 6:
                    result = "Third Temple";
                    break;
                case 7:
                    result = "Third Temple";
                    break;
                case 8:
                    result = "Glass Port";
                    break;
                case 9:
                    result = "Hanging Gardens";
                    break;
                case 10:
                    result = "Hanging Gardens";
                    break;
                case 11:
                    result = "Heaven's Edge";
                    break;
                case 12:
                    result = "Heaven's Edge";
                    break;
                case 13:
                    result = "Custom";
                    break;
                case 14:
                    result = "Lower Heaven";
                    break;
                case 15:
                    result = "Lower Heaven";
                    break;
                case 16:
                    result = "Lower Heaven";
                    break;
                case 17:
                    result = "Old City";
                    break;
                case 18:
                    result = "Glass Port";
                    break;
                case 19:
                    result = "Glass Port";
                    break;
            }

            return result;
        }

        public static void ObjectSetup(GameObject[] objects)
        {
            foreach (GameObject obj in objects)
            {
                string[] objectData = obj.name.Split('/');
                string objType = objectData[0];

                obj.AddComponent<ObjectSpawner>();
                ObjectSpawner spawner = obj.GetComponent<ObjectSpawner>();
                spawner.spawnOnStart = false;

                bool barrelType = false;

                if (objType.EndsWith("_Standard"))
                {
                    barrelType = false;
                    objType = objType.Replace("_Standard", "");
                }
                else if (obj.name.EndsWith("_Big"))
                {
                    barrelType = true;
                    objType = objType.Replace("_Big", "");
                }

                switch (objType)
                {
                    case "Spawn_Object_Goal":
                        spawner._objectType = ObjectSpawner.Type.Goal;
                        spawner.Spawn();
                        break;
                    case "Spawn_Object_ExplosiveBarrel":
                        spawner._objectType = ObjectSpawner.Type.ExplosiveBarrel;

                        if (barrelType) spawner.barrelType = BreakableBarrel.BarrelType.Big;
                        else spawner.barrelType = BreakableBarrel.BarrelType.Standard;

                        spawner.Spawn();
                        break;
                    case "Spawn_Object_BreakablePlatform":
                        spawner._objectType = ObjectSpawner.Type.BreakablePlatform;
                        spawner.Spawn();
                        break;
                    case "Spawn_Object_EnvironmentPortal":
                        spawner._objectType = ObjectSpawner.Type.EnvironmentPortal;

                        EnvironmentPortal.EnvironmentPortalData epd = new EnvironmentPortal.EnvironmentPortalData();
                        epd.curseOnTeleport = false;
                        epd.portalActor = actorData;
                        epd.pType = EnvironmentPortal.EnvironmentPortalData.PortalType.SameLevelEnvironment;
                        epd.teleportID = string.Concat("TELEPORT_", objectData[1]);

                        spawner._environmentPortalData = epd;

                        spawner.Spawn();
                        break;
                    case "Spawn_Object_DarkInsight":
                        spawner._objectType = ObjectSpawner.Type.DarkInsight;
                        spawner.Spawn();
                        break;
                }
            }
        }
    }
}
