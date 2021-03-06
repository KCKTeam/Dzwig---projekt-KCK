﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;


public class GetText : MonoBehaviour {
	public Text craneResponse;
	public Text playerText;
	public Transform TextContainer;

	//tablica przechowująca wszystkie obiekty ze sceny
	public GameObject [] obiekty;

	Slownik slownik=new Slownik();
	AnalizaZapytania analiza;
	Zapytanie nowe;

	InputField wejscie;
	public Transform grupaObiektow;

	void Awake(){
		obiekty = GameObject.FindGameObjectsWithTag ("obiekt");
	}
	void Start(){
		//wczytanie wszystkich obiektów do tablicy
		wejscie = GetComponent<InputField> ();
		analiza = gameObject.AddComponent (typeof(AnalizaZapytania)) as AnalizaZapytania;
	}

	public void text(){
		string text = wejscie.text;//zmienna text przechowuje polecenie wpisane w Unity
		myText(text); //wyswietla w oknie gry tekst wpisany przez uzytkownika
		wejscie.text="";

		int iloscDopytan = analiza.iloscDopytan ();
		if (iloscDopytan > 0) {
			analiza.dopytaj (text);
		} else {
			nowe = new Zapytanie (text);
			analiza.dodajObiekty (obiekty);
			analiza.dodajZapytanie (nowe);
			analiza.dodajSlownik (slownik);

			analiza.znajdzTokeny ();
			analiza.znajdzObiekty ();

			iloscDopytan = analiza.iloscDopytan ();
			if (iloscDopytan > 0) {
				analiza.dopytaj ("abcdefghijkl");
			} else {
				//analiza.uzupelnijKoloryRodzaje ();
				analiza.znajdzPolecenie ();
			}
		}
	}

	public void craneText(string text){
		Text newText=Instantiate (craneResponse, TextContainer, worldPositionStays:false) as Text;
		newText.text = text;
	}

	public void myText(string text){
		Text newText=Instantiate (playerText, TextContainer, worldPositionStays:false) as Text;
		newText.text = text;
	}

}