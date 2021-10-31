using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IIIrdQuestion : MonoBehaviour
{
    [SerializeField] Transform player, path, pathParent, coin;
    Transform prevPath;
    int xFac, zFac, xPlayerFac, zPlayerFac, directionFactor, coinRotated;
    public static int score=0;
    float spawnFac = 0.2f, removalFac = 1;

    void Start()
    {
        xFac = xPlayerFac = directionFactor = coinRotated = 0;
        zFac = zPlayerFac = 1;
        prevPath = path;
        StartCoroutine(BuildPath());
        StartCoroutine(ChangeFactors());
    }

    IEnumerator ChangeFactors()
    {
        yield return new WaitForSeconds(5);
        directionFactor = Random.Range(-1, 2);
        if(directionFactor==-1)
        {
            if(xFac==0 && zFac==1)
            {
                xFac = -1;
                zFac = 0;
            }
            else if (xFac == -1 && zFac == 0)
            {
                xFac = 0;
                zFac = -1;
            }
            else if (xFac == 0 && zFac == -1)
            {
                xFac = 1;
                zFac = 0;
            }
            else if (xFac == 1 && zFac == 0)
            {
                xFac = 0;
                zFac = 1;
            }
        }
        else if (directionFactor == 1)
        {
            if (xFac == 0 && zFac == 1)
            {
                xFac = 1;
                zFac = 0;
            }
            else if (xFac == 1 && zFac == 0)
            {
                xFac = 0;
                zFac = -1;
            }
            else if (xFac == 0 && zFac == -1)
            {
                xFac = -1;
                zFac = 0;
            }
            else if (xFac == -1 && zFac == 0)
            {
                xFac = 0;
                zFac = 1;
            }
        }
        coinRotated += 90 * directionFactor;
        StartCoroutine(ChangeFactors());
    }
    IEnumerator BuildPath()
    {
        while (pathParent.childCount >= 1000)
            yield return new WaitForSeconds(1);

        Transform newPath = Instantiate(path, pathParent);
        newPath.localPosition = prevPath.localPosition + new Vector3(5*xFac, 0, 5*zFac);
        newPath.rotation = Quaternion.Euler(0, 0, 0);
        Transform newCoin =  Instantiate(coin);
        newCoin.position = new Vector3(newPath.position.x, 0.8f, newPath.position.z);
        newCoin.rotation = Quaternion.Euler(0, coinRotated, 0);
        prevPath = newPath;
        yield return new WaitForSeconds(spawnFac);
        StartCoroutine(BuildPath());
    }
    IEnumerator ClearLoad()
    {
        Destroy(pathParent.GetChild(0).gameObject);
        yield return new WaitForSeconds(removalFac);
        StartCoroutine(ClearLoad());
    }

    void Update()
    {
        player.position += new Vector3(xPlayerFac, 0, zPlayerFac) * 0.3f;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (xPlayerFac == 0 && zPlayerFac == 1)
            {
                xPlayerFac = -1;
                zPlayerFac = 0;
                player.rotation = Quaternion.Euler(Vector3.up * -90);
            }
            else if (xPlayerFac == -1 && zPlayerFac == 0)
            {
                xPlayerFac = 0;
                zPlayerFac = -1;
                player.rotation = Quaternion.Euler(Vector3.up * -180);
            }
            else if (xPlayerFac == 0 && zPlayerFac == -1)
            {
                xPlayerFac = 1;
                zPlayerFac = 0;
                player.rotation = Quaternion.Euler(Vector3.up * -270);
            }
            else if (xPlayerFac == 1 && zPlayerFac == 0)
            {
                xPlayerFac = 0;
                zPlayerFac = 1;
                player.rotation = Quaternion.Euler(Vector3.up * 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (xPlayerFac == 0 && zPlayerFac == 1)
            {
                xPlayerFac = 1;
                zPlayerFac = 0;
                player.rotation = Quaternion.Euler(Vector3.up * 90);
            }
            else if (xPlayerFac == 1 && zPlayerFac == 0)
            {
                xPlayerFac = 0;
                zPlayerFac = -1;
                player.rotation = Quaternion.Euler(Vector3.up * 180);
            }
            else if (xPlayerFac == 0 && zPlayerFac == -1)
            {
                xPlayerFac = -1;
                zPlayerFac = 0;
                player.rotation = Quaternion.Euler(Vector3.up * 270);
            }
            else if (xPlayerFac == -1 && zPlayerFac == 0)
            {
                xPlayerFac = 0;
                zPlayerFac = 1;
                player.rotation = Quaternion.Euler(Vector3.up * 0);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Coins")
        {
            Destroy(collision.gameObject);
            score++;
            if(score == 10)
                StartCoroutine(ClearLoad());
        }
        else if (collision.gameObject.tag == "Fallen")
        {
            SceneManager.LoadScene(0);
        }
    }
}
