﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Drones.UI
{
    using Utils;
    public abstract class DashboardPanel : MonoBehaviour
    {
        protected static Dictionary<DashboardMode, Vector2> ModeSize = new Dictionary<DashboardMode, Vector2>
        {
            {DashboardMode.EditMode, new Vector2(399, 60)},
            {DashboardMode.Simulation, new Vector2(550, 345)}
        };

        protected Dictionary<Transform, Button> _OwnerToButton;

        protected abstract Dictionary<Transform, Button> OwnerToButton { get; }

        protected GameObject _ActiveFoldable;

        protected void EnableFoldable(Button button)
        {
            if (_ActiveFoldable != null && _ActiveFoldable.gameObject.activeSelf)
            {
                OwnerToButton[_ActiveFoldable.transform.parent].onClick.Invoke();
            }
            _ActiveFoldable = button.transform.GetChild(1).gameObject;
            _ActiveFoldable.SetActive(true);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate { DisableFoldable(button); });
        }

        protected void DisableFoldable(Button button)
        {
            _ActiveFoldable.SetActive(false);
            _ActiveFoldable = null;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate { EnableFoldable(button); });
        }
    }

}