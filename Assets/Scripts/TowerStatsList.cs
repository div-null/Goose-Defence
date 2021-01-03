
public abstract class TowerStatsList
{
	public abstract string Name { get; }
	public abstract string Discription { get; }
	public abstract float AttackDelay { get; }
	public abstract ProjectileStats Projectile { get; }
	public abstract int MaxHP { get; }
	public abstract float Range { get; }
	public abstract float Cost { get; }
	public abstract float FreezeCoef { get; }
	public abstract int PrefabId { get; }

	const float SredDMG = 800f;
	const float SredMaxHP = 1500f;
	const float SredSpeed = 4f;
	const float SredAttackCountPer20Sec = 10f;
	const float SredAttackFar = 20f;
	const float SredAttackSpeed = 2f;

	//Дамаг
	const float T2CoefDMG = 3f;
	const float T3CoefDMG = 5f;
	const float WallCoefDMG = 0f;

	//Дальность атаки
	const float T2CoefAttackFar = 1.7f;
	const float T3CoefAttackFar = 2.4f;
	const float WallCoefAttackFar = 0f;

	//АОЕ радиус
	const float T2CoefAttackAOE = 1.7f;
	const float T3CoefAttackAOE = 2.4f;
	const float WallCoefAttackAOE = 0f;

	//Макс ХП
	const float T2MaxHP = 2f;
	const float T3MaxHP = 3f;
	const float WallMaxHP = 20f;

	//Скорость снаряда (секунды)
	const float T2FlightSpeed = 1.5f;
	const float T3FlightSpeed = 2f;
	const float WallFlightSpeed = 0f;

	//Скорость атаки
	const float T2AttackSpeed = 0.85f;
	const float T3AttackSpeed = 0.7f;
	const float WallAttackSpeed = 0f;

	//
	const float TomatoDMGCoef = 1f;
	const float CabbageDMGCoef = 0.8f;
	const float PeasDMGCoef = 0.3f;

	const float TomatoHPCoef = 1f;
	const float CabbageHPCoef = 0.6f;
	const float PeasHPCoef = 0.15f;

	const float TomatoFarCoef = 1.3f;
	const float CabbageFarCoef = 1f;
	const float PeasFarCoef = 1.7f;

	const float TomatoSpeedCoef = 1.2f;
	const float CabbageSpeedCoef = 0.8f;
	const float PeasSpeedCoef = 1.5f;

	const float TomatoAOECoef = 0.1f;
	const float CabbageAOECoef = 1f;
	const float PeasAOECoef = 1.5f;

	const float TomatoAttackSpeedCoef = 1f;
	const float CabbageAttackSpeedCoef = 2f;
	const float PeasAttackSpeedCoef = 1.2f;

	const float CostT1 = 1000f;
	const float CostT2 = 2500f;
	const float CostT3 = 4000f;

	public static TowerStatsList GetStatsByPrefabId(int PrefabId)
	{
		switch (PrefabId)
		{
			case 0: return new TowerTomatoT1();
			case 1: return new TowerTomatoT2();
			case 2: return new TowerTomatoT3();
			case 3: return new TowerCabbageT1();
			case 4: return new TowerCabbageT2();
			case 5: return new TowerCabbageT3();
			case 6: return new TowerPeasT1();
			case 7: return new TowerPeasT2();
			case 8: return new TowerPeasT3();
			case 9: return new Wall();
			case 10: return new Colocol();

			default:
				return new Wall();
		}
	}
	//Томатная башня
	//Стреляет одиночными помидорами, кормит одного гуся.
	//{

	//Attack delay: 2
	//Projectile: 
	//	Damage: 400
	//	Explosion range: 0,1
	//	Velocity: 4,8
	//	Coef slow: 1
	//	Slow time: 0
	//MaxHP: 1500
	//Range: 26
	//Cost: 1000
	//Freeze: 0
	//}
public class TowerTomatoT1 : TowerStatsList
	{
		public override string Name => "Томатная башня";
		public override string Discription => "Стреляет одиночными помидорами, кормит одного гуся.";
		public override float AttackDelay => SredAttackSpeed * TomatoAttackSpeedCoef;
		public override ProjectileStats Projectile => 
				new ProjectileStats((int)( SredDMG * TomatoDMGCoef / ( SredAttackSpeed * TomatoAttackSpeedCoef ) ),
									TomatoAOECoef,
									SredSpeed * TomatoSpeedCoef);
		public override int MaxHP => (int)( SredMaxHP * TomatoHPCoef );
		public override float Range => SredAttackFar * TomatoFarCoef;
		public override float Cost => CostT1;
		public override float FreezeCoef => 0;
		public override int PrefabId => 0;
	}

	//Томатная башня Т2
	//Стреляет одиночными помидорами, кормит одного гуся.
	//{

	//Attack delay: 1,7
	//Projectile: 
	//	Damage: 1411
	//	Explosion range: 0,17
	//	Velocity: 7,2
	//	Coef slow: 1
	//	Slow time: 0
	//MaxHP: 3000
	//Range: 44,2
	//Cost: 2500
	//Freeze: 0
	//}
	public class TowerTomatoT2 : TowerStatsList
	{
		public override string Name => "Томатная башня Т2";
		public override string Discription => "Стреляет одиночными помидорами, кормит одного гуся.";
		public override float AttackDelay => SredAttackSpeed * TomatoAttackSpeedCoef * T2AttackSpeed;
		public override ProjectileStats Projectile => new ProjectileStats((int)( SredDMG * TomatoDMGCoef * T2CoefDMG / ( SredAttackSpeed * TomatoAttackSpeedCoef * T2AttackSpeed ) ), TomatoAOECoef * T2CoefAttackAOE, SredSpeed * TomatoSpeedCoef * T2FlightSpeed);
		public override int MaxHP => (int)( SredMaxHP * TomatoHPCoef * T2MaxHP );
		public override float Range => SredAttackFar * TomatoFarCoef * T2CoefAttackFar;
		public override float Cost => CostT2;
		public override float FreezeCoef => 0;
		public override int PrefabId => 1;
	}

	//Томатная башня Т3
	//Стреляет одиночными помидорами, кормит одного гуся.
	//{

	//Attack delay: 1,4
	//Projectile: 
	//	Damage: 2857
	//	Explosion range: 0,24
	//	Velocity: 9,6
	//	Coef slow: 1
	//	Slow time: 0
	//MaxHP: 4500
	//Range: 62,4
	//Cost: 4000
	//Freeze: 0
	//}
	public class TowerTomatoT3 : TowerStatsList
	{
		public override string Name => "Томатная башня Т3";
		public override string Discription => "Стреляет одиночными помидорами, кормит одного гуся.";
		public override float AttackDelay => SredAttackSpeed * TomatoAttackSpeedCoef * T3AttackSpeed;
		public override ProjectileStats Projectile => new ProjectileStats((int)( SredDMG * TomatoDMGCoef * T3CoefDMG / ( SredAttackSpeed * TomatoAttackSpeedCoef * T3AttackSpeed ) ), TomatoAOECoef * T3CoefAttackAOE, SredSpeed * TomatoSpeedCoef * T3FlightSpeed);
		public override int MaxHP => (int)( SredMaxHP * TomatoHPCoef * T3MaxHP );
		public override float Range => SredAttackFar * TomatoFarCoef * T3CoefAttackFar;
		public override float Cost => CostT3;
		public override float FreezeCoef => 0;
		public override int PrefabId => 2;
	}

	//Капустная башня
	//Стреляет большой капустой, кормит несколько гусей.
	//{

	//Attack delay: 4
	//Projectile: 
	//	Damage: 160
	//	Explosion range: 1
	//	Velocity: 3,2
	//	Coef slow: 1
	//	Slow time: 0
	//MaxHP: 900
	//Range: 20
	//Cost: 1000
	//Freeze: 0
	//}
public class TowerCabbageT1 : TowerStatsList
	{
		public override string Name => "Капустная башня";
		public override string Discription => "Стреляет большой капустой, кормит несколько гусей.";
		public override float AttackDelay => SredAttackSpeed * CabbageAttackSpeedCoef;
		public override ProjectileStats Projectile => new ProjectileStats((int)( SredDMG * CabbageDMGCoef / ( SredAttackSpeed * CabbageAttackSpeedCoef ) ), CabbageAOECoef, SredSpeed * CabbageSpeedCoef);
		public override int MaxHP => (int)( SredMaxHP * CabbageHPCoef );
		public override float Range => SredAttackFar * CabbageFarCoef;
		public override float Cost => CostT1;
		public override float FreezeCoef => 0;
		public override int PrefabId => 3;
	}

	//Томатная башня Т2
	//Стреляет большой капустой, кормит несколько гусей.
	//{

	//Attack delay: 3,4
	//Projectile: 
	//	Damage: 564
	//	Explosion range: 1,7
	//	Velocity: 4,8
	//	Coef slow: 1
	//	Slow time: 0
	//MaxHP: 1800
	//Range: 34
	//Cost: 2500
	//Freeze: 0
	//}
	public class TowerCabbageT2 : TowerStatsList
	{
		public override string Name => "Томатная башня Т2";
		public override string Discription => "Стреляет большой капустой, кормит несколько гусей.";
		public override float AttackDelay => SredAttackSpeed * CabbageAttackSpeedCoef * T2AttackSpeed;
		public override ProjectileStats Projectile => new ProjectileStats((int)( SredDMG * CabbageDMGCoef * T2CoefDMG / ( SredAttackSpeed * CabbageAttackSpeedCoef * T2AttackSpeed ) ), CabbageAOECoef * T2CoefAttackAOE, SredSpeed * CabbageSpeedCoef * T2FlightSpeed);
		public override int MaxHP => (int)( SredMaxHP * CabbageHPCoef * T2MaxHP );
		public override float Range => SredAttackFar * CabbageFarCoef * T2CoefAttackFar;
		public override float Cost => CostT2;
		public override float FreezeCoef => 0;
		public override int PrefabId => 4;
	}

	//Томатная башня Т3
	//Стреляет большой капустой, кормит несколько гусей.
	//{

	//Attack delay: 2,8
	//Projectile: 
	//	Damage: 1142
	//	Explosion range: 2,4
	//	Velocity: 6,4
	//	Coef slow: 1
	//	Slow time: 0
	//MaxHP: 2700
	//Range: 48
	//Cost: 4000
	//Freeze: 0
	//}
	public class TowerCabbageT3 : TowerStatsList
	{
		public override string Name => "Томатная башня Т3";
		public override string Discription => "Стреляет большой капустой, кормит несколько гусей.";
		public override float AttackDelay => SredAttackSpeed * CabbageAttackSpeedCoef * T3AttackSpeed;
		public override ProjectileStats Projectile => new ProjectileStats((int)( SredDMG * CabbageDMGCoef * T3CoefDMG / ( SredAttackSpeed * CabbageAttackSpeedCoef * T3AttackSpeed ) ), CabbageAOECoef * T3CoefAttackAOE, SredSpeed * CabbageSpeedCoef * T3FlightSpeed);
		public override int MaxHP => (int)( SredMaxHP * CabbageHPCoef * T3MaxHP );
		public override float Range => SredAttackFar * CabbageFarCoef * T3CoefAttackFar;
		public override float Cost => CostT3;
		public override float FreezeCoef => 0;
		public override int PrefabId => 5;
	}
	//Гороховая башня
	//Стреляет большим количеством горошин, кормит несколько гусей.Дополнительно замедляет их.
	//{

	//Attack delay: 2,4
	//Projectile:
	//	Damage: 100
	//	Explosion range: 1,5
	//	Velocity: 6
	//	Coef slow: 0,8
	//	Slow time: 2
	//MaxHP: 225
	//Range: 34
	//Cost: 1000
	//Freeze: 0,7
	//}
	public class TowerPeasT1 : TowerStatsList
	{
		public override string Name => "Гороховая башня";
		public override string Discription => "Стреляет большим количеством горошин, кормит несколько гусей. Дополнительно замедляет их.";
		public override float AttackDelay => SredAttackSpeed * PeasAttackSpeedCoef;
		public override ProjectileStats Projectile => 
						new ProjectileStats((int)( SredDMG * PeasDMGCoef / ( SredAttackSpeed * PeasAttackSpeedCoef ) ),
											PeasAOECoef, 
											SredSpeed * PeasSpeedCoef,
											0.8f,
											2f);
		public override int MaxHP => (int)( SredMaxHP * PeasHPCoef );
		public override float Range => SredAttackFar * PeasFarCoef;
		public override float Cost => CostT1;
		public override float FreezeCoef => 0.7f;
		public override int PrefabId => 6;
	}
	//Гороховая башня Т2
	//Стреляет большим количеством горошин, кормит несколько гусей.Дополнительно замедляет их.
	//{

	//Attack delay: 2,04
	//Projectile: 
	//	Damage: 352
	//	Explosion range: 2,55
	//	Velocity: 9
	//	Coef slow: 0,7
	//	Slow time: 2,5
	//MaxHP: 450
	//Range: 57,8
	//Cost: 2500
	//Freeze: 0,6
	//}
public class TowerPeasT2 : TowerStatsList
	{
		public override string Name => "Гороховая башня Т2";
		public override string Discription => "Стреляет большим количеством горошин, кормит несколько гусей. Дополнительно замедляет их.";
		public override float AttackDelay => SredAttackSpeed * PeasAttackSpeedCoef * T2AttackSpeed;
		public override ProjectileStats Projectile => new ProjectileStats((int)( SredDMG * PeasDMGCoef * T2CoefDMG / ( SredAttackSpeed * PeasAttackSpeedCoef * T2AttackSpeed ) ), PeasAOECoef * T2CoefAttackAOE, SredSpeed * PeasSpeedCoef * T2FlightSpeed, 0.7f, 2.5f);
		public override int MaxHP => (int)( SredMaxHP * PeasHPCoef * T2MaxHP );
		public override float Range => SredAttackFar * PeasFarCoef * T2CoefAttackFar;
		public override float Cost => CostT2;
		public override float FreezeCoef => 0.6f;
		public override int PrefabId => 7;
	}

	//Гороховая башня Т3
	//Стреляет большим количеством горошин, кормит несколько гусей.Дополнительно замедляет их.
	//{

	//Attack delay: 1,68
	//Projectile: 
	//	Damage: 714
	//	Explosion range: 3,6
	//	Velocity: 12
	//	Coef slow: 0,7
	//	Slow time: 3
	//MaxHP: 675
	//Range: 81,60001
	//Cost: 4000
	//Freeze: 0,5
	//}
public class TowerPeasT3 : TowerStatsList
	{
		public override string Name => "Гороховая башня Т3";
		public override string Discription => "Стреляет большим количеством горошин, кормит несколько гусей. Дополнительно замедляет их.";
		public override float AttackDelay => SredAttackSpeed * PeasAttackSpeedCoef * T3AttackSpeed;
		public override ProjectileStats Projectile => new ProjectileStats((int)( SredDMG * PeasDMGCoef * T3CoefDMG / ( SredAttackSpeed * PeasAttackSpeedCoef * T3AttackSpeed ) ), PeasAOECoef * T3CoefAttackAOE, SredSpeed * PeasSpeedCoef * T3FlightSpeed, 0.7f, 3f);
		public override int MaxHP => (int)( SredMaxHP * PeasHPCoef * T3MaxHP );
		public override float Range => SredAttackFar * PeasFarCoef * T3CoefAttackFar;
		public override float Cost => CostT3;
		public override float FreezeCoef => 0.5f;
		public override int PrefabId => 8;
	}

	//Стена
	//Очень колючая и сухая.
	//{

	//Attack delay: 1,68
	//Projectile: 
	//	Damage: 0
	//	Explosion range: 0
	//	Velocity: 0
	//	Coef slow: 0
	//	Slow time: 0
	//MaxHP: 30000
	//Range: 0
	//Cost: 20000
	//Freeze: 0
	//}
public class Wall : TowerStatsList
	{
		public override string Name => "Стена";
		public override string Discription => "Очень колючая и сухая.";
		public override float AttackDelay => SredAttackSpeed * PeasAttackSpeedCoef * T3AttackSpeed;
		public override ProjectileStats Projectile => new ProjectileStats(0, 0f, 0f, 0f, 0f);
		public override int MaxHP => (int)( WallMaxHP * SredMaxHP );
		public override float Range => 0;
		public override float Cost => 20000;
		public override float FreezeCoef => 0;
		public override int PrefabId => 9;
	}

	//Колокол
	//Блестящий и звонкий.
	//{

	//Attack delay: 1,68
	//Projectile: 
	//	Damage: 0
	//	Explosion range: 0
	//	Velocity: 0
	//	Coef slow: 0
	//	Slow time: 0
	//MaxHP: 1000000
	//Range: 0
	//Cost: 0
	//Freeze: 0
	//}
	public class Colocol : TowerStatsList
	{
		public override string Name => "Колокол";
		public override string Discription => "Блестящий и звонкий.";
		public override float AttackDelay => SredAttackSpeed * PeasAttackSpeedCoef * T3AttackSpeed;
		public override ProjectileStats Projectile => new ProjectileStats(0, 0f, 0f, 0f, 0f);
		public override int MaxHP => (int)( 1000000 );
		public override float Range => 0;
		public override float Cost => 0;
		public override float FreezeCoef => 0;
		public override int PrefabId => 10;
	}

}
