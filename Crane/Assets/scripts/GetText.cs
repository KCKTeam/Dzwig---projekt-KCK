﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;


public class GetText : MonoBehaviour {
	
	//tablica przechowująca wszystkie obiekty ze sceny
	public GameObject [] obiekty;

	//słowa kluczowe
	string [] kolory = {"ziel","niebi","czerw", "fiolet", "żółt", "biał", "czarn"};
	string [] rodzaje = {"becz", "kontene", "skrzyn","samoch", "karto", "drzwia", "okn", "pił" };
	string [] czas_pod = {"podnieś", "unieś"};
	string [] czas_kladz = {"opuść", "postaw","odłóż", "połóż"};
	string [] zaimki = {"obok", "lew","praw", "na", "przed", "za"};

	InputField wejscie;
	void Start(){
		//wczytanie wszystkich obiektów do tablicy
		obiekty = GameObject.FindGameObjectsWithTag ("obiekt");
		for (int i = 0; i < obiekty.Length; i++) {
			Debug.Log (obiekty [i].GetComponent<objectProperties> ().rodzaj);
		}

		wejscie = GetComponent<InputField> ();
	}
		
	public void text(){
		string text = wejscie.text; //zmienna text przechowuje polecenie wpisane w Unity
		searchOrder(text);
	}


	bool czy_trzymam=false;
	void searchOrder(string pobrany_tekst){
		bool obrot = false, czy_liczba = false;
		bool czy_podnoszenie = false, czy_kladzenie = false;
		string kierunek = "";
		float kat = 0;
		//zmienne do przetworzenia tekstu
		char delimiter = ' ';
		string[] substrings = pobrany_tekst.ToLower ().Split (delimiter);

		//----------------------obracanie dźwigu---------------------//
		foreach (string substring in substrings) {
			if (substring == "obróć") {
				obrot = true;
			}
			if (Regex.IsMatch (substring, @"^\d+$")) {
				czy_liczba = true;
				float.TryParse (substring, out kat);
			}
			if (substring == "obróć") {
				obrot = true;
			}

			if (substring == "lewo" || substring.ToLower () == "prawo") {
				kierunek = substring.ToLower ();
			}
		}

		if (obrot && czy_liczba) {
			if (kierunek == "lewo") {
				kat *= -1;
			}

			Debug.Log ("Obracam dźwig o "+ kat.ToString() +" stopni");
			StartCoroutine(CraneManager.Instance.rotation(kat));
		}
		//--------------------------------------------------------------//

		//----wyszukiwanie czasowników podnoszenia i opuszczania----//
		foreach (string substring in substrings) {
			for (int i = 0; i < czas_pod.Length; i++) {
				if (substring.Contains (czas_pod [i])) {
					Debug.Log ("Podnieś: " + substring);
					czy_podnoszenie = true;
				}
			}

			for (int i = 0; i < czas_kladz.Length; i++) {
				if (substring.Contains (czas_kladz [i])) {
					Debug.Log ("Opuść: " + substring);
					czy_kladzenie = true;
				}
			}
		}
		//---------------------------------------------------------//

		//----sprawdzenie, którą funkcję należy wywołać----//
		if (czy_podnoszenie && czy_kladzenie) {
			Debug.Log ("Podnosze i klade");
		}else if ((czy_podnoszenie) && (!czy_trzymam)) {
			czy_trzymam = true;
			Debug.Log ("Tylko podnosze");
		}else if (czy_kladzenie && czy_trzymam) {
			Debug.Log ("Tylko klade");
		}
		//------------------------------------------------//

			
		//----sprawdzenie koloru, rodzaju i zaimków---------//
		foreach (string substring in substrings) {
			for (int i = 0; i < kolory.Length; i++) {
				if (substring.Contains (kolory [i])) {
					Debug.Log ("Twój kolor: " + substring);
				}
			}

			for (int i = 0; i < rodzaje.Length; i++) {
				if (substring.Contains (rodzaje [i])) {
					Debug.Log ("Twój rodzaj: " + substring);
				}
			}


			for (int i = 0; i < zaimki.Length; i++) {
				if (substring.Contains (zaimki [i])) {
					Debug.Log ("Twój zaimek: " + substring);
				}
			}
		}
		//------------------------------------------------------//
			
	}
}
	