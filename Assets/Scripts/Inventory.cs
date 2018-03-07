﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	GameObject inventoryPanel; // Holds slot panel
	GameObject slotPanel; // Holds slots
	public GameObject inventorySlot; // Prefab instance of an inventory slot
	public GameObject inventoryItem; // Prefab instance of an inventory item

	int numSlots;

	public List<AdventureItem> allItems; // Holds all the item instances for the inventory
	public List<GameObject> allSlots; // Holds all the slot instances for the inventory

	ItemDatabase itemDB;

	// Use this for initialization
	void Start () {
	
		allItems = new List<AdventureItem> ();
		allSlots = new List<GameObject> ();
		itemDB = GetComponent<ItemDatabase>();

		inventoryPanel = GameObject.Find ("inventoryPanel");
		slotPanel = inventoryPanel.transform.Find ("slotPanel").gameObject;

		numSlots = 20;
		for (int i = 0; i < numSlots; i++) {
			allItems.Add (new AdventureItem ()); // Add empty item
			allSlots.Add (Instantiate (inventorySlot)); // Create instance of slot prefab
			allSlots [i].transform.SetParent (slotPanel.transform); // Set correct parent
			allSlots[i].GetComponent<Slot>().ID = i; // Set ID of slot
		}

		addItem (0);
		addItem (1);
		addItem (2);
		addItem (3);
		addItem (4);
		addItem (5);

	}

	public void addItem(int id) {
		AdventureItem itemToAdd = itemDB.getItem (id);

		if (itemToAdd.IsStackable && itemAlreadyExists(itemToAdd)) {
			for (int i = 0; i < allItems.Count; i++) {
				if (allItems [i].ID == id) {
					allSlots [i].transform.GetChild (0).GetComponent<ItemData> ().increaseAmt(1); // Update associated item data
					return;
				}
			}
		}

		for (int i = 0; i < allItems.Count; i++) {
			if (allItems [i].ID == -1) { // Check for 'empty slot'
				allItems [i] = itemToAdd; // Assign empty slot to new item
				GameObject itemObject = Instantiate (inventoryItem); // Create instance of item prefab

				itemObject.GetComponent<ItemData>().init(itemToAdd, i); // Initialize itemData
				itemObject.transform.SetParent (allSlots [i].transform); // Set correct parent
				itemObject.transform.position = Vector2.zero; // Center item in slot
				itemObject.GetComponent<Image>().sprite = itemToAdd.Sprite; // Replace default sprite w/ item sprite
				itemObject.name = itemToAdd.Title; // Set name of prefab to name of item(for convenience)
				return;
			}
		}
	}

	bool itemAlreadyExists(AdventureItem item) {

		for (int i = 0; i < allItems.Count; i++) {
			if (allItems [i].ID == item.ID) {
				return true;
				// Return index instead, then use that, this circumvents redundantly looping thrrough the items again
			}
		}
		return false;
		// Otherwise return -1(bad item id)
	}

	// Update is called once per frame
	void Update () {}
}