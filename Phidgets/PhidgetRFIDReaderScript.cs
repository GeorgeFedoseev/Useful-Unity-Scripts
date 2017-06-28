using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Phidgets;
using Phidgets.Events;

public class PhidgetRFIDReaderScript : MonoBehaviour {

	public bool isAttached;
	public string currentRFID;

	RFID rfid;

	// Use this for initialization
	void Start () {
		try
		{
			rfid = new RFID();
			rfid.Attach += new AttachEventHandler(rfid_Attach);
			rfid.Detach += new DetachEventHandler(rfid_Detach);
			rfid.Error += new ErrorEventHandler(rfid_Error);
			//   rfid.OutputChange += new OutputChangeEventHandler(rfid_output);
			rfid.Tag += new TagEventHandler(rfid_Tag);
			rfid.TagLost += new TagEventHandler(rfid_TagLost);

			Loom.RunAsync(() => {
				rfid.open();	
			});


			//Wait for a Phidget RFID to be attached before doing anything with 
			//the object
			//Debug.Log("waiting for attachment...");
			//StartCoroutine(WaitForAttachmentForPhidget());
			//turn on the antenna and the led to show everything is working

		}

		catch (PhidgetException ex)
		{
			Debug.Log(ex.Description);
		}
	}

	// METHODS
	
	IEnumerator WaitForAttachmentForPhidget()
	{
		rfid.waitForAttachment();

		yield return new WaitForSeconds(0.1f);

	}



	// EVENTS

	void rfid_Tag(object sender, TagEventArgs e)
	{
		string  s = string.Format("Tag {0}" ,e.Tag.ToString());
		Debug.Log(s);

		Loom.QueueOnMainThread(() => {
			currentRFID = e.Tag.ToString();
		});
	}

	//print the tag code for the tag that was just lost
	void rfid_TagLost(object sender, TagEventArgs e)
	{
		string s = string.Format("Tag  {0} IS LOST", e.Tag.ToString());
		Debug.Log(s);

		Loom.QueueOnMainThread(() => {
			currentRFID = "";
		});
	}

	//attach event handler...display the serial number of the attached RFID phidget
	void rfid_Attach(object sender, AttachEventArgs e)
	{
		string s = string.Format("RFIDReader {0} attached", e.Device.SerialNumber.ToString());

		Debug.Log(s);

		isAttached = true;


		// enable scanning
		rfid.Antenna = true;
		rfid.LED = true;
	}

	//detach event handler...display the serial number of the detached RFID phidget
	void rfid_Detach(object sender, DetachEventArgs e)
	{

		string s = string.Format("RFIDReader {0} detached", e.Device.SerialNumber.ToString());

		Debug.Log(s);

		isAttached = false;
	}

	//Error event handler...display the error description string
	void rfid_Error(object sender, ErrorEventArgs e)
	{
		Debug.LogError("RFID reader error: "+e.Description);
	}
}
