using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class Gerenciador : MonoBehaviour {

public List<Vector3> pontos = new List<Vector3>();
public List<Vector2> solucao = new List<Vector2>();
public int qtdeIlhas = 0;
public int qtdeLuzesAcesas = 0;
public float qtdeMediaLuzIlha = 0.0f;
private bool isOk = true;
public bool randomPlay = true;
public int dimensao = 20;

public GameObject pedra;
private GameObject[,] tabuleiro;
public List<string> acoes = new List<string>();
private Vector2 scrollPosition = Vector2.zero;


	public List<Vector2> Solve () {
	List<Vector2> s = new List<Vector2>();
	for(var i=0;i<dimensao;i++)
	{
		for(var j=0;j<dimensao;j++)
		{
		tabuleiro[i,j].GetComponent<Pedra>().AlternaEstado(true);
			if(checkIsOk()==true)
			{
			s.Add(new Vector2(i,j));
			break;		
			}
			else
			{
			
			tabuleiro[i,j].GetComponent<Pedra>().AlternaEstado(true);
			}
		}
	}
	return s;
	}

	public int[,] ToInt(GameObject[,] tabuleiro) {
	int[,] saida = new int[dimensao,dimensao];
	for(int i=0;i<dimensao;i++)
	{
		for(int j=0;j<dimensao;j++)
		{
		saida[i,j] = tabuleiro[i,j].GetComponent<Pedra>().getIsAtivo() ? 1 : 0;		
		}
	}
	return saida;
	}

	public int getDimensao () {
	return dimensao;	
	}

	public GameObject[,] getTabuleiro () {
	return tabuleiro;
	}

	void Start () {
	pontos.Add(Vector3.zero);
	acoes.Add("Criando tabuleiro " + dimensao.ToString() + "x" + dimensao.ToString());
	acoes.Add("[sub]");
	Camera.main.orthographicSize += 0.1f;	
	tabuleiro = new GameObject[dimensao,dimensao];
	for(int i=0;i<dimensao;i++)
	{
		for(int j=0;j<dimensao;j++)
		{
		tabuleiro[i,j] = Instantiate(pedra, new Vector3(i, 0, j), Quaternion.identity);
		tabuleiro[i,j].GetComponent<Pedra>().init(i,j);
		}	
	}
	if(randomPlay==true)
	{
		//acoes.Add("Inicializando tabuleiro aleatoriamente");	
		for(int i=0;i<dimensao;i++)
		{
			for(var j=0;j<dimensao;j++)
			{
				//if(Random.Range(0,2)%2==0)
				//{
				if((i==1 && j==2) || (i==2 && j==2) || (i==2 && j==1))
				{
				tabuleiro[i,j].GetComponent<Pedra>().AlternaEstado(false);
				}
				//}
			}
		}	
		//acoes.Add("Fim da inicialização aleatória");	
	acoes.Add("[sub]");
	}
	Vector3 middlePos = dimensao%2==0 ? tabuleiro[dimensao/2,dimensao/2].transform.position - (new Vector3(0.5f,0.0f,0.5f)) : tabuleiro[dimensao/2,dimensao/2].transform.position;
	Camera.main.transform.position = new Vector3(middlePos.x,Camera.main.transform.position.y,middlePos.z);
	Camera.main.orthographicSize = dimensao/2.0f;
	qtdeIlhas = contaIlhas(ToInt(tabuleiro));
	qtdeLuzesAcesas = contaLuzesAcesas(ToInt(tabuleiro));
	qtdeMediaLuzIlha = ((qtdeLuzesAcesas+0.0f)/(qtdeIlhas+0.0f));
	WatchChange();
	}
	
	public void WatchChange () {
	Vector3 novoValor = new Vector3(qtdeIlhas,qtdeLuzesAcesas,qtdeMediaLuzIlha);
	if(novoValor!=pontos[pontos.Count-1])
	{
	pontos.Add(novoValor);
	}
	}

	void Update () {
	qtdeIlhas = contaIlhas(ToInt(tabuleiro));
	qtdeLuzesAcesas = contaLuzesAcesas(ToInt(tabuleiro));
	qtdeMediaLuzIlha = ((qtdeLuzesAcesas+0.0f)/(qtdeIlhas+0.0f));
	isOk = checkIsOk();
	}

	public void Eval () {
	
	}

	public bool ehResolvivel () {
	return true;	
	}
	
	void OnGUI () {
	if(GUI.Button(new Rect(5,5,100,25),"New Game"))
	{
	Scene scene = SceneManager.GetActiveScene(); 
	SceneManager.LoadScene(scene.name);
	}
	if(GUI.Button(new Rect(5,35,100,25),"Solve"))
	{
	solucao = Solve();
	//solver.Solve();
	}
	if(GUI.Button(new Rect(5,65,50,25),"<"))
	{

	}
	if(GUI.Button(new Rect(55,65,50,25),">"))
	{
	//Eval(acoesDesfeitas.pop());
	}
	
	GUI.Label(new Rect(5,90,200,25),"Qtde. Ilhas = " + qtdeIlhas.ToString());
	GUI.Label(new Rect(5,110,200,25),"Qtde. Luzes Acesas = " + qtdeLuzesAcesas.ToString());
	GUI.Label(new Rect(5,130,200,25),"Luzes por ilha = " + qtdeMediaLuzIlha.ToString());
	GUI.Label(new Rect(5,150,200,25),"É resolvível = " + ehResolvivel().ToString());

	GUI.Box(new Rect((Screen.width*2)/3,0,Screen.width/3,Screen.height),"Ações");

	scrollPosition = GUI.BeginScrollView(new Rect((Screen.width*2)/3,35,Screen.width/3,Screen.height), scrollPosition, new Rect(0, 0, Screen.width/3,acoes.Count*20));
	int k = 0;
	int j = 0;
	int l = 0;	
	for(int i=0;i<acoes.Count;i++)
	{
	//GUI.Label(new Rect(5,i*20,Screen.width/3-5,20),(acoes[i].Contains("[sub]") ? "" : prefixo + " ") + acoes[i].Replace("[sub]",""));
	j += acoes[i]=="[sub]" ? (j==0 ? 1 : -j-1) : 0;
	k += j==0 ? 1 : 0;
	l = acoes[i]=="[sub]" ? 0 : l+1;
	GUI.Label(new Rect(5,i*20,Screen.width/3-5,20),(j==0 ? k.ToString() : acoes[i]=="[sub]" ? "" : k.ToString()+"."+l.ToString()) + " >>\t" + acoes[i]);	
	j = j==-1 ? 0 : j;
	}
	GUI.EndScrollView();
	}

	public bool checkIsOk () {
	bool s = true;
	for(int i=0;i<dimensao;i++)
	{
		for(int j=0;j<dimensao;j++)
		{
			if(tabuleiro[i,j].GetComponent<Pedra>().getIsAtivo()==true)
			{
			s = false;
			break;
			}
		}
	}	
	return s;
	}

	bool isSafe(int[,] M, int linha, int coluna, bool[,] visitados) {
    return (linha >= 0) && (linha < dimensao) && (coluna >= 0) && (coluna < dimensao) && (M[linha,coluna]==1 && !visitados[linha,coluna]);
    }
 
    void DFS(int[,] M, int linha, int coluna, bool[,] visitados) {
	int[] eixoY = new int[]  {-1,0,0,1};
	int[] eixoX = new int[]  {0,-1,1,0};
    
	visitados[linha,coluna] = true;
 
	for (int k = 0; k < eixoY.Length; ++k)
	{
    	if (isSafe(M, linha + eixoY[k], coluna + eixoX[k], visitados))
		{
		DFS(M, linha + eixoY[k], coluna + eixoX[k], visitados);
		}
    }
	}

    int contaLuzesAcesas (int[,] M) {
	bool[,] visitados = new bool[dimensao,dimensao];
	int cont = 0;
	for (int i = 0; i < dimensao; i++)
		for (int j = 0; j < dimensao; j++)
			if (M[i,j]==1)
			{                                
			cont++;			
			}
	return cont;
    }

 
    int contaIlhas(int[,] M) {
	bool[,] visitados = new bool[dimensao,dimensao];
	int cont = 0;
	for (int i = 0; i < dimensao; ++i)
		for (int j = 0; j < dimensao; ++j)
			if (M[i,j]==1 && !visitados[i,j])
			{                                
			DFS(M, i, j, visitados);
			++cont;
			}
	return cont;
    }
}
