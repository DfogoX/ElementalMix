using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public GameObject bullet;

    public float shootForce, upForce, maxDistance, bulletLifetime;

    public float shootingInterval, bulletSpread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;

    public bool allowButtonHold;

    private int bulletsLeft, bulletsShot;
    private bool shooting, readyToShoot, reloading;
    
    public Camera cam;
    public Transform attackPoint;

    public GameObject muzzleFlash;
    public float muzzleDuration;
    public TextMeshProUGUI ammunitionDisplay; 
    
    public bool allowInvoke = true;
    
    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void MyInput()
    {

        shooting = allowButtonHold ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");

        if (Input.GetButton("Reload") && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            Reload();
        }
        
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            
            Shoot();
        }
        
        
    }

    private void Shoot()
    {
        readyToShoot = false;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(maxDistance);
        }

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        var x = Random.Range(-bulletSpread, bulletSpread);
        var y = Random.Range(-bulletSpread, bulletSpread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        Destroy(currentBullet, bulletLifetime);

        currentBullet.transform.forward = directionWithSpread.normalized;
        
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(cam.transform.up * upForce, ForceMode.Impulse);

        if (muzzleFlash != null)
        {
            var muzzle = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
            Destroy(muzzle, muzzleDuration);
            
        }
        
        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", shootingInterval);
            allowInvoke = false;
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
        
    }

    void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
    
    void Update()
    {

        MyInput();

        if (ammunitionDisplay != null)
        {
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        }
        
    }

    
}
