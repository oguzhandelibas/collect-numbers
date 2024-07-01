#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using CollectNumbers;
using UnityEngine;
using UnityEditor;

namespace ODProjects.LevelEditor
{
    public class LevelEditor : EditorWindow
    {
        #region DATAS

        private LevelData[] _allLevelDatas;
        private LevelData _currentLevelData;
        private ColorData _colorData;
        //private ElementData _elementData;

        #endregion

        #region ENUMS

        private SelectedNumber _selectedNumber;
        private SelectedColor _selectedColor;

        #endregion

        #region VARIABLES
        
        private int _targetCountTemp=0;
        private bool _targetColorsInitialized;
        private int _targetCount;
        private int _boxSize = 25;
        private bool _hasInitialize;
        private string[] _levelDataNames;
        private int _selectedOption = 0;

        #endregion

        #region MAIN FUNCTIONS

        [MenuItem("OD Projects/Mobile/LevelEditor", false, 1)]
        public static void ShowCreatorWindow()
        {
            LevelEditor window = GetWindow<LevelEditor>();

            window.titleContent = new GUIContent("Level Editor");
            window.titleContent.image = EditorGUIUtility.IconContent("d_Animation.EventMarker").image;
            window.titleContent.tooltip = "Car Lot Jam Level Editor, by OD";
            window.Focus();
        }

        #endregion

        #region GUI FUNCTIONS

        private void LoadLevelDatas()
        {
            string levelDataFolder = "Assets/Resources/LevelData";
            if (Directory.Exists(levelDataFolder))
            {
                string[] assetPaths = Directory.GetFiles(levelDataFolder, "*.asset");
                List<LevelData> levelDataList = new List<LevelData>();
                List<string> levelDataNameList = new List<string>();

                foreach (string path in assetPaths)
                {
                    LevelData levelData = AssetDatabase.LoadAssetAtPath<LevelData>(path);
                    if (levelData != null)
                    {
                        levelDataList.Add(levelData);
                        levelDataNameList.Add(levelData.name);
                    }
                }

                _allLevelDatas = levelDataList.ToArray();
                _levelDataNames = levelDataNameList.ToArray();
                if (_currentLevelData == null) _currentLevelData = levelDataList[0];
            }
            else
            {
                _allLevelDatas = new LevelData[0];
            }
        }
        private Vector2 scrollPosition = Vector2.zero;

        private void OnGUI()
        {
            LoadLevelDatas();
            
            CheckPathAndInitialization();

            int maxGridSize = 14;
            _boxSize = 80;
            if (_currentLevelData.gridSize.x > maxGridSize) _currentLevelData.gridSize.x = maxGridSize;
            if (_currentLevelData.gridSize.y > maxGridSize) _currentLevelData.gridSize.y = maxGridSize;
            if (_currentLevelData.gridSize.x > 6) _boxSize = 65;
            if (_currentLevelData.gridSize.x > 8) _boxSize = 45;

            GUI.color = Color.white;

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition); // Scrollview ba�lat

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (_currentLevelData != null) Content();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.EndScrollView(); // Scrollview'� sonland�r

            GUI.color = Color.green;
            
            if (GUILayout.Button("CREATE NEW LEVEL", GUILayout.Height(40)))
            {
                _currentLevelData = ScriptableObject.CreateInstance<LevelData>();
                string levelDataFolder = "Assets/Resources/LevelData";
                string[] assetPaths = Directory.GetFiles(levelDataFolder, "*.asset");
                _selectedOption = assetPaths.Length;

                string levelName = "LevelData_" + (assetPaths.Length + 1);
                string path = "Assets/Resources/LevelData/" + levelName + ".asset";
                AssetDatabase.CreateAsset(_currentLevelData, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private void CheckPathAndInitialization()
        {
            if (!_hasInitialize)
            {
                _colorData = Resources.Load<ColorData>("ColorData");
                //_elementData = Resources.Load<ElementData>("ElementData");
                if (!_currentLevelData.HasPath)
                {
                    _currentLevelData.SetArray(_currentLevelData.gridSize.x * _currentLevelData.gridSize.y);
                }
                _hasInitialize = true;
                _selectedColor = SelectedColor.Red;
            }
        }
        
        private void Content()
        {
            EditorGUILayout.Space();

            GUILayout.Label("Level Editor", EditorStyles.boldLabel);

            EditorGUILayout.Space();

            LevelDropdown();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("box", GUILayout.Width(400));
            _currentLevelData = (LevelData)EditorGUILayout.ObjectField("Level Data", _currentLevelData, typeof(LevelData), false);
            _colorData = (ColorData)EditorGUILayout.ObjectField("Color Data", _colorData, typeof(ColorData), false);
            //_elementData = (ElementData)EditorGUILayout.ObjectField("Element Data", _elementData, typeof(ElementData), false);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("box", GUILayout.Width(300));
            _currentLevelData.moveCount = EditorGUILayout.IntField("Move Count", _currentLevelData.moveCount);
            
            _targetCount = EditorGUILayout.IntField("Goal Colors Count", _targetCount);
            if (_targetCount != _targetCountTemp)
            {
                _currentLevelData.Goals.Clear();
                for (int i = 0; i < _targetCount; i++)
                {
                    _currentLevelData.Goals.Add(new Goal());
                    EditorGUILayout.BeginVertical("box", GUILayout.Width(200));
                    var editorGoal = _currentLevelData.Goals[i];
                    editorGoal.TargetColor = (SelectedColor)EditorGUILayout.EnumPopup("Color", editorGoal.TargetColor);
                    editorGoal.Count = EditorGUILayout.IntField("Value", editorGoal.Count);
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }
                _targetCountTemp = _targetCount;
            }
            else
            {
                if(_currentLevelData.Goals.Count == 0) return;
                for (int i = 0; i < _targetCount; i++)
                {
                    EditorGUILayout.BeginHorizontal("box", GUILayout.Width(200));
                    var goal = _currentLevelData.Goals[i];
                    goal.TargetColor = (SelectedColor)EditorGUILayout.EnumPopup("Color", goal.TargetColor);
                    goal.Count = EditorGUILayout.IntField("Value", goal.Count);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(50);
            
            EditorGUILayout.BeginVertical("box", GUILayout.Width(300));
            _selectedNumber = (SelectedNumber)EditorGUILayout.EnumPopup("Selected Element", _selectedNumber);
            //_selectedColor = (SelectedColor)EditorGUILayout.EnumPopup("Selected Color", _selectedColor);
            switch (_selectedNumber)
            {
                case SelectedNumber.Null:
                    _selectedColor = SelectedColor.Null;
                    break;
                case SelectedNumber.One:
                    _selectedColor = SelectedColor.Red;
                    break;
                case SelectedNumber.Two:
                    _selectedColor = SelectedColor.Green;
                    break;
                case SelectedNumber.Three:
                    _selectedColor = SelectedColor.Blue;
                    break;
                case SelectedNumber.Four:
                    _selectedColor = SelectedColor.Orange;
                    break;
                case SelectedNumber.Five:
                    _selectedColor = SelectedColor.Purple;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            EditorGUILayout.Space();

            GridArea();
            CreateGrid();
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(50);
            

            GUI.color = Color.red;
            if (GUILayout.Button("CLEAR LEVEL", GUILayout.Height(30)))
            {
                _currentLevelData.ClearPath();
                _hasInitialize = false;
            }


            GUI.color = Color.white;
            EditorUtility.SetDirty(_currentLevelData);
        }

        private void LevelDropdown()
        {
            GUILayout.Label("Dropdown Example", EditorStyles.boldLabel);

            int newSelectedOption = EditorGUILayout.Popup("Select a Level:", _selectedOption, _levelDataNames);

            if (_selectedOption != newSelectedOption)
            {
                _selectedOption = newSelectedOption;
                _currentLevelData = _allLevelDatas[_selectedOption];
                _hasInitialize = false;
            }
        }
        
        private void GridArea()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Grid Size", GUILayout.Width(75));
            Vector2Int newGridSize = EditorGUILayout.Vector2IntField("", _currentLevelData.gridSize);

            if (GUILayout.Button("Reset Grid", GUILayout.Height(25)))
            {
                _currentLevelData.ResetGrid();
                newGridSize = _currentLevelData.gridSize;
            }

            EditorGUILayout.EndHorizontal();
            if (newGridSize.y > 9) newGridSize.y = 9;
            if (newGridSize.x > 9) newGridSize.x = 9;
            if (newGridSize.y < 2) newGridSize.y = 2;
            if (newGridSize.x < 2) newGridSize.x = 2;


            if (_currentLevelData.gridSize.x != newGridSize.x || _currentLevelData.gridSize.y != newGridSize.y)
            {
                _currentLevelData.gridSize.x = newGridSize.x;
                _currentLevelData.gridSize.y = newGridSize.y;
                _currentLevelData.gridSize.x = newGridSize.x;
                _currentLevelData.gridSize.y = newGridSize.y;

                _hasInitialize = false;
                _currentLevelData.ClearPath();
                CheckPathAndInitialization();
            }
            /*
            EditorGUILayout.LabelField("Button Size", GUILayout.Width(75));
            _boxSize =  EditorGUILayout.IntField(_boxSize);*/


            EditorGUILayout.Space();

        }

        private void CreateGrid()
        {
            if (_currentLevelData.gridSize.x < 1)
                _currentLevelData.gridSize.x = 1;
            if (_currentLevelData.gridSize.y < 1)
                _currentLevelData.gridSize.y = 1;

            float totalWidth = _currentLevelData.gridSize.x * _boxSize;
            float startX = (position.width - totalWidth) / 2;
            GUIContent content = new GUIContent("N/A");
            GUI.color = Color.white;

            for (int y = 0; y < _currentLevelData.gridSize.y; y++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space((position.width - totalWidth) / 2);
                for (int x = 0; x < _currentLevelData.gridSize.x; x++)
                {
                    int index = y * _currentLevelData.gridSize.x + x;
                    
                    if (index >= 0 && index < _currentLevelData.ArrayLength())
                    {
                        GridButton(content, index, x, y);
                    }
                }

                GUI.color = Color.white;
                GUILayout.Space((position.width - totalWidth) / 2);
                EditorGUILayout.EndHorizontal();
            }
        }

        private void GridButton(GUIContent content, int index, int x, int y)
        {
            GUI.color = _currentLevelData.GetColor(index);
            content = _currentLevelData.GetContent(index);
            //content.text = x + ", " + y;
            //content.text = index.ToString();

            if (GUI.Button(GUILayoutUtility.GetRect(_boxSize, _boxSize), content, GUI.skin.button))
            {
                bool hasNeighbour = true;
                if (_selectedColor == SelectedColor.Null || _selectedNumber == SelectedNumber.Null) // ERASE
                {
                    content.text = "N/A";
                    _currentLevelData.SetButtonColor(index, SelectedColor.Null, _colorData.Colors[SelectedColor.Null].color, content, SelectedNumber.Null);
                }

                if (!hasNeighbour) return;
                else if(_selectedColor != SelectedColor.Null && _selectedNumber != SelectedNumber.Null)  // ADD
                {
                    ChangeButtonState(content, index);
                    //content.image = _elementData.Elements[_selectedElement];
                }
            }
        }

        private void ChangeButtonState(GUIContent content, int index)
        {
            List<int> indexes = new List<int>();
            if (_currentLevelData.ElementIsAvailable(index)) indexes.Add(index);
            else indexes.Clear();
            
            if (indexes.Count > 0)
            {
                _currentLevelData.SetButtonColor(index, _selectedColor, _colorData.Colors[_selectedColor].color, content, _selectedNumber);
                /*for (int i = 1; i < indexes.Count; i++)
                {
                    _currentLevelData.SetFakeButtonColor(indexes[i], _selectedColor, _colorData.Colors[_selectedColor].color, content, _selectedElement);
                }*/
                string temp1 = _selectedNumber.ToString();
                content.text = temp1;
            }
        }

        #endregion
    }
}
#endif