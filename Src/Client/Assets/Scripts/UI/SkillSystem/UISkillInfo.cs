using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using Common.Data;
using Models;
using SkillBridge.Message;
using Managers;

public class UISkillInfo : MonoBehaviour
{
    public Text title;
    public Text description;
    public Text descriptionNextLevel;
    public Text updateCost;
    public Button updateButton;
    public ListView listMain;
}