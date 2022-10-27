using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Variables
    [Header("Button Object")]
    [SerializeField]
    private Button colorMenuBtn;
    [SerializeField]
    private Button upgradeMenuBtn;
    [SerializeField]
    private Button upgradeMenuReturnBtn;
    [SerializeField]
    private Button colorMenuReturnBtn;

    [Header("Move Position")]
    [SerializeField]
    private Transform upgradeMenuIn;
    [SerializeField]
    private Transform upgradeMenuOut;
    [SerializeField]
    private Transform bottomMenuIn;
    [SerializeField]
    private Transform bottomMenuOut;
    [SerializeField]
    private Transform sideMenuIn;
    [SerializeField]
    private Transform sideMenuOut;
    [SerializeField]
    private Transform battleMenuIn;
    [SerializeField]
    private Transform battleMenuOut;
    [SerializeField]
    private Transform mainCameraZoomIn;
    [SerializeField]
    private Transform mainCameraZoomOut;


    [Header("Move Object")]
    [SerializeField]
    private GameObject colorMenuObject;
    [SerializeField]
    private GameObject upgradeMenuObject;
    [SerializeField]
    private GameObject bottomMenuObject;
    [SerializeField]
    private GameObject sideMenuObject;
    [SerializeField]
    private GameObject battleMenuObject;
    [SerializeField]
    private Camera mainCamera;

    private float moveTimer = 1f;
    #endregion

    #region BuiltIn Methods
    private void Start()
    {
        colorMenuBtn.onClick.AddListener(() => MenuClick(colorMenuObject, upgradeMenuIn, true));
        colorMenuReturnBtn.onClick.AddListener(() => MenuClick(colorMenuObject, upgradeMenuOut, false));
        upgradeMenuBtn.onClick.AddListener(() => MenuClick(upgradeMenuObject, upgradeMenuIn, true));
        upgradeMenuReturnBtn.onClick.AddListener(() => MenuClick(upgradeMenuObject, upgradeMenuOut, false));
    }
#endregion

    #region Custom Methods
    void MenuClick(GameObject objectForMoving, Transform transformToMove, bool isIn)
    {
        objectForMoving.transform.DOMove(transformToMove.position, moveTimer);

        if (isIn)
        {
            MainCameraZoom(mainCameraZoomIn);
            BottomAndSideMenuMove(bottomMenuObject,bottomMenuOut);
            BottomAndSideMenuMove(sideMenuObject,sideMenuOut);
            BottomAndSideMenuMove(battleMenuObject,battleMenuOut);
        }
        else
        {
            MainCameraZoom(mainCameraZoomOut);
            BottomAndSideMenuMove(bottomMenuObject,bottomMenuIn);
            BottomAndSideMenuMove(sideMenuObject,sideMenuIn);
            BottomAndSideMenuMove(battleMenuObject,battleMenuIn);
        }
    }
    void BottomAndSideMenuMove(GameObject objectForMoving, Transform transformToMove)
    {
        objectForMoving.transform.DOMove(transformToMove.position, moveTimer);
    }

    void MainCameraZoom(Transform transformToMove)
    {
        mainCamera.transform.DOMove(transformToMove.position, moveTimer);
        mainCamera.transform.DORotateQuaternion(transformToMove.rotation, moveTimer);
    }
    #endregion


}