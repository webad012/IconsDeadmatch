#pragma strict

//private var pickUpsController : PickUpsController;

function Start () 
{
	//GameObject go = GameObject.FindGameObjectsWithTag("PickUpsController");
	//pickUpsController = (PickUpsController) go.GetComponent(typeof(PickUpsController));
}

function OnTriggerEnter(other : Collider)
{
	if(other.gameObject.tag == "Player")
	{
		var pickUpsController = GameObject.FindWithTag("PickUpsController");
    	pickUpsController.GetComponent(PickUpsController).PickedUp();
		Destroy(gameObject);
	}
}