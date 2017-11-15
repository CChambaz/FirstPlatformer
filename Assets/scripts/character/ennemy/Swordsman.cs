using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman : Characters
{
    public Swordsman()
    {
        force_x = 250;
        force_y = 25;
        mass = 65;

        max_life = 1;
        max_health_point = 5;
        max_ammo = -1;

        life = max_life;
        health_point = max_health_point;
        ammo = max_ammo;
    }
}
