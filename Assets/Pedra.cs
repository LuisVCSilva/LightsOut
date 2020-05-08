using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Pedra : MonoBehaviour {
    AudioSource audioSource;
	public AudioClip beep;
	private int dimensao = 0;
	private bool isAtivo = false;
	private int i = 0;
	private int j = 0;

	public bool getIsAtivo () {
	return isAtivo;
	}

	public void init (int _i, int _j) {
	i = _i;
	j = _j;
	OnMouseExit();
	}

	void Awake() {
	audioSource = GetComponent<AudioSource>();
	}

	void Start () {
	dimensao = Camera.main.GetComponent<Gerenciador>().getDimensao();
	}

	void OnMouseEnter () {
	if(isAtivo==false)
	{
	gameObject.GetComponent<Renderer>().material.color = Color.green;	
	}
	}

	void OnMouseExit () {
	if(isAtivo==false)
	{
	gameObject.GetComponent<Renderer>().material.color = Color.grey;	
	}
	}
	
	void OnMouseDown () {
	AlternaEstado(true);
	}

	public void AlternaEstado (bool isRecursivo) {	
	audioSource.PlayOneShot(beep, 0.7F);
	isAtivo = !isAtivo;
	Camera.main.GetComponent<Gerenciador>().acoes.Add("bloco[" + i.ToString()+","+j.ToString()+"] = " + (isAtivo ? "True" : "False"));
	GetComponent<Renderer>().material.color = isAtivo ? Color.yellow : Color.grey;
	if(isRecursivo)
	{
	Camera.main.GetComponent<Gerenciador>().acoes.Add("[sub]");	
		if(i-1>=0)
		{
		Camera.main.GetComponent<Gerenciador>().getTabuleiro()[i-1,j].GetComponent<Pedra>().AlternaEstado(false);
		}
		if(i+1<dimensao)
		{
		Camera.main.GetComponent<Gerenciador>().getTabuleiro()[i+1,j].GetComponent<Pedra>().AlternaEstado(false);
		}
		if(j-1>=0)
		{
		Camera.main.GetComponent<Gerenciador>().getTabuleiro()[i,j-1].GetComponent<Pedra>().AlternaEstado(false);
		}
		if(j+1<dimensao)
		{
		Camera.main.GetComponent<Gerenciador>().getTabuleiro()[i,j+1].GetComponent<Pedra>().AlternaEstado(false);
		}
	Camera.main.GetComponent<Gerenciador>().acoes.Add("[sub]");
	}
	else
	{
	Camera.main.GetComponent<Gerenciador>().WatchChange();	
	}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
