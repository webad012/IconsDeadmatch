#pragma strict

private var comboBox : ComboBox;
private var list : GUIContent[];
private var listStyle : GUIStyle;
private var comboBoxRect : Rect;
private var startButtonRect : Rect;
private var selectedComboBox : int;

function Start()
{
	list = new GUIContent[2];
	list[0] = new GUIContent("Zvezda vs Partizan");
	list[1] = new GUIContent("Partizan vs Zvezda");
	
	startButtonRect = Rect(Screen.width/2 - 100, Screen.height/2 - 100, 200, 30);
	comboBoxRect = Rect(Screen.width/2 - 100, Screen.height/2, 200, 20);
	
	// Make a GUIStyle that has a solid white hover/onHover background to indicate highlighted items
	listStyle = new GUIStyle();
	listStyle.normal.textColor = Color.white;
	var tex = new Texture2D(2, 2);
	var colors = new Color[4];
	for (color in colors) color = Color.white;
	tex.SetPixels(colors);
	tex.Apply();
	listStyle.hover.background = tex;
	listStyle.onHover.background = tex;
	listStyle.padding.left = listStyle.padding.right = listStyle.padding.top = listStyle.padding.bottom = 4;
	
	comboBox = ComboBox(comboBoxRect, list[PlayerPrefs.GetInt ("SelectedOption", 0)], list, listStyle);
}

function OnGUI()
{
	selectedComboBox = comboBox.Show();
	
	if(GUI.Button(startButtonRect, "Start"))
	{
		PlayerPrefs.SetInt("SelectedOption", selectedComboBox);
		Application.LoadLevel("Default");
	}
}