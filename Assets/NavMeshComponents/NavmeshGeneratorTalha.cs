using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshGeneratorTalha : MonoBehaviour
{
	public NavMeshSurface surface;

	public void NavmeshStart()
	{
		surface.BuildNavMesh();
	}

}
