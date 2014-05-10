#pragma strict

public var numberOfPickUps : int;
public var pickupsText : GUIText;

function OnGUI()
{
	pickupsText.text = "Left: " + numberOfPickUps;
}

public function PickedUp()
{
	numberOfPickUps--;
}

function Start () {

}

function Update () {

}