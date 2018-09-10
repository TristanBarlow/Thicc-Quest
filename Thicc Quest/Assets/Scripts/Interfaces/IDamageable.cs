using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

interface IDamageable
{
     void Hit(float dmg, AffinityData aff);
     void Healed(float dmg, AffinityData aff);
}
