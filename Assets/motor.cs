using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class motor : MonoBehaviour
{
    public List<GameObject> cubes;
    public List<GameObject> centers;
    public List<GameObject> topms_gobjs;
    public List<GameObject> topms;
    public List<GameObject> topcs;
    public List<GameObject> topws;
    public GameObject byz_kontrol;
    public GameObject sag_ust_orta_kontrol_SAG;
    public GameObject sag_ust_orta_kontrol_SOL;
    public GameObject sag_ust_orta_kontrol_ON;
    public GameObject sag_ust_orta_kontrol_ARKA;

    /*private void Start()
    {
        StartCoroutine(RotateSequence());

    }*/
    GameObject FindCubeByName(string name)
    {
        return cubes.Find(cube => cube.name == name);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Baslat());
        }
    }

    IEnumerator Baslat()
    {
        yield return StartCoroutine(RotateSequence());
        Debug.Log("Karılma işlemi tamamlandı");
        yield return StartCoroutine(Ustte_arti_olustur());
        Debug.Log("Üstte + işareti oluşturma tamamlandı");
        yield return StartCoroutine(Hareket1());
        Debug.Log("Orta üst ve merkez küpler hizalandı");
        yield return StartCoroutine(Hareket2());
        Debug.Log("Üst satır tamamlandı");

        sag_asagi(); yield return Bekle(); // ters çevir
        orta_asagi(); yield return Bekle();
        sol_asagi(); yield return Bekle();
        sag_asagi(); yield return Bekle();
        orta_asagi(); yield return Bekle();
        sol_asagi(); yield return Bekle();

        yield return StartCoroutine(Hareket3());
        Debug.Log("Orta satır tamamlandı");
        yield return StartCoroutine(Hareket4());
        Debug.Log("Üstte + işareti oluşturma tamamlandı");
        yield return StartCoroutine(Hareket5());
        Debug.Log("Orta üst ve merkez küpler hizalandı");
        yield return StartCoroutine(Hareket6());
        Debug.Log("Küp tamamlandı");

        //en sonda satırları uygun renklere göre hizala
        GameObject cube11 = FindCubeByName("11");
        GameObject cube19 = FindCubeByName("19");
        GameObject cube2 = FindCubeByName("2");
        while (cube2.transform.position.x != 1 || cube2.transform.position.z != 1)
        {
            alt_saga(); yield return Bekle();
        }
        while (cube11.transform.position.x != 1 || cube11.transform.position.z != 1)
        {
            orta_saga(); yield return Bekle();
        }
        while (cube19.transform.position.x != 1 || cube19.transform.position.z != 1)
        {
            ust_saga(); yield return Bekle();
        }
    }

    IEnumerator Ustte_arti_olustur()
    {
        Debug.Log("BAŞLA");
        GameObject cube4 = FindCubeByName("4");
        int i = 0;
        while (cube4.transform.position.y != 1 && i != 4)
        {
            der_orta_saga(); yield return Bekle();
            i++;
        }
        if (i == 4)
        {
            while (cube4.transform.position.y != 1)
            {
                orta_asagi(); yield return Bekle();
            }
        }

        GameObject cube7 = FindCubeByName("7");
        GameObject cube5 = FindCubeByName("5");
        GameObject cube3 = FindCubeByName("3");
        GameObject cube1 = FindCubeByName("1");

        Vector3 euler7 = cube7.transform.rotation.eulerAngles;
        Vector3 euler5 = cube5.transform.rotation.eulerAngles;
        Vector3 euler3 = cube3.transform.rotation.eulerAngles;
        Vector3 euler1 = cube1.transform.rotation.eulerAngles;

        Vector3 pos7 = cube7.transform.position;
        Vector3 pos5 = cube5.transform.position;
        Vector3 pos3 = cube3.transform.position;
        Vector3 pos1 = cube1.transform.position;

        yield return StartCoroutine(arti_olustur_ara_islem(cube1, 0));
        yield return StartCoroutine(arti_olustur_ara_islem(cube3, 1));
        yield return StartCoroutine(arti_olustur_ara_islem(cube5, 2));
        yield return StartCoroutine(arti_olustur_ara_islem(cube7, 3));

    }

    IEnumerator arti_olustur_ara_islem(GameObject cube, int ws_ind)
    {
        if (byz_kontrol_results(cube, ws_ind).ust)
        {
            //Debug.Log(cube.name + " yerleştirildi");
        }
        else
        {
            while (cube.transform.position.y != 0)
            {
                if (cube.transform.position.z == 1)
                {
                    while (!ust_kenar_bos_kontrol(sag_ust_orta_kontrol_ON))
                    {
                        ust_sola(); yield return Bekle();
                    }
                    on_saga(); yield return Bekle();
                }
                if (cube.transform.position.z == -1)
                {
                    while (!ust_kenar_bos_kontrol(sag_ust_orta_kontrol_ARKA))
                    {
                        ust_sola(); yield return Bekle();
                    }
                    arka_saga(); yield return Bekle();
                }

                if (cube.transform.position.x == 1)
                {
                    while (!ust_kenar_bos_kontrol(sag_ust_orta_kontrol_SOL))
                    {
                        ust_sola(); yield return Bekle();
                    }
                    sol_asagi(); yield return Bekle();
                }
                if (cube.transform.position.x == -1)
                {
                    while (!ust_kenar_bos_kontrol(sag_ust_orta_kontrol_SAG))
                    {
                        ust_sola(); yield return Bekle();
                    }
                    sag_asagi(); yield return Bekle();
                }
            }

            if (cube.transform.rotation.eulerAngles.x == 0 && cube.transform.rotation.eulerAngles.z == 0 && cube.transform.position.y == 1)
            {
                yield break;
            }

            while (cube.transform.position.x != 1 || cube.transform.position.z != 1)
            {
                orta_sola(); yield return Bekle();
            }

            while (!ust_kenar_bos_kontrol(sag_ust_orta_kontrol_SAG))
            {
                ust_sola(); yield return Bekle();
            }

            if (byz_kontrol_results(cube, ws_ind).on)
            {
                orta_saga(); yield return Bekle();
                orta_saga(); yield return Bekle();
                sag_asagi(); yield return Bekle();
            }
            else if (byz_kontrol_results(cube, ws_ind).sol)
            {
                orta_saga(); yield return Bekle();
                sag_yukari(); yield return Bekle();
            }
        }
    }

    bool ust_kenar_bos_kontrol(GameObject kontrol)
    {
        return Vector3.Distance(kontrol.transform.position, topws[0].transform.position) > 0.05f &&
                Vector3.Distance(kontrol.transform.position, topws[1].transform.position) > 0.05f &&
                Vector3.Distance(kontrol.transform.position, topws[2].transform.position) > 0.05f &&
                Vector3.Distance(kontrol.transform.position, topws[3].transform.position) > 0.05f;
    }

    (bool sol, bool sag, bool ust, bool alt, bool on, bool arka) byz_kontrol_results(GameObject cube, int ws_ind)
    {
        Vector3 cubePos = cube.transform.position;
        Vector3 targetPos = topws[ws_ind].transform.position;

        bool ust = Vector3.Distance(cubePos + Vector3.up, targetPos) - 0.5f < 0.05f;
        bool alt = Vector3.Distance(cubePos + Vector3.down, targetPos) - 0.5f < 0.05f;
        bool sag = Vector3.Distance(cubePos + Vector3.left, targetPos) - 0.5f < 0.05f;
        bool sol = Vector3.Distance(cubePos + Vector3.right, targetPos) - 0.5f < 0.05f;
        bool on = Vector3.Distance(cubePos + Vector3.forward, targetPos) - 0.5f < 0.05f;
        bool arka = Vector3.Distance(cubePos + Vector3.back, targetPos) - 0.5f < 0.05f;

        return (sol, sag, ust, alt, on, arka);
    }


    IEnumerator Hareket6()
    {
        GameObject cube17 = FindCubeByName("17");
        GameObject cube19 = FindCubeByName("19");
        GameObject cube23 = FindCubeByName("23");
        GameObject cube25 = FindCubeByName("25");

        GameObject cube12 = FindCubeByName("12");
        GameObject cube10 = FindCubeByName("10");
        GameObject cube13 = FindCubeByName("13");
        GameObject cube15 = FindCubeByName("15");

        int j = 0;
        GameObject uygun = null;
        while (true)
        {
            uygun = null;
            j = 0;
            if (uygunMuHareket6(cube17, cube10, cube12))
            {
                uygun = cube17;
                j++;
            }
            if (uygunMuHareket6(cube19, cube10, cube13))
            {
                uygun = cube19;
                j++;
            }
            if (uygunMuHareket6(cube23, cube15, cube12))
            {
                uygun = cube23;
                j++;
            }
            if (uygunMuHareket6(cube25, cube13, cube15))
            {
                uygun = cube25;
                j++;
            }

            if (j == 4)
            {
                break;
            }
            else
            {
                if (uygun == null)
                {
                    yield return StartCoroutine(hareket6_algoritma());
                }
                else
                {
                    while (uygun.transform.position.x != 1 || uygun.transform.position.z != 1)
                    {
                        ust_saga(); yield return Bekle();
                        orta_saga(); yield return Bekle();
                        alt_saga(); yield return Bekle();
                    }
                    yield return StartCoroutine(hareket6_algoritma());
                }
            }
        }

        Dictionary<GameObject, GameObject> koseler = new Dictionary<GameObject, GameObject>
        {
            { cube17, topcs[0] },
            { cube19, topcs[1] },
            { cube23, topcs[2] },
            { cube25, topcs[3] }
        };

        foreach (KeyValuePair<GameObject, GameObject> pair in koseler)
        {
            GameObject cube = pair.Key;
            GameObject top = pair.Value;

            while (cube.transform.position.x != -1 || cube.transform.position.z != 1)
            {
                ust_saga(); yield return Bekle();
            }

            while (Mathf.Abs(top.transform.position.y - 1.5f) >= 0.05f)
            {
                sag_asagi(); yield return Bekle();
                alt_sola(); yield return Bekle();
                sag_yukari(); yield return Bekle();
                alt_saga(); yield return Bekle();
            }
        }

    }

    bool uygunMuHareket6(GameObject cube, GameObject komsu1, GameObject komsu2)
    {
        return Vector3.Distance(cube.transform.position, komsu1.transform.position) - 1.41f < 0.05f && Vector3.Distance(cube.transform.position, komsu2.transform.position) - 1.41f < 0.05f;
    }

    IEnumerator hareket6_algoritma()
    {
        sag_yukari(); yield return Bekle();
        ust_saga(); yield return Bekle();
        sol_yukari(); yield return Bekle();
        ust_sola(); yield return Bekle();
        sag_asagi(); yield return Bekle();
        ust_saga(); yield return Bekle();
        sol_asagi(); yield return Bekle();
        ust_sola(); yield return Bekle();
    }

    IEnumerator Hareket5()
    {
        // Küpler
        GameObject cube18 = FindCubeByName("18");
        GameObject cube20 = FindCubeByName("20");
        GameObject cube22 = FindCubeByName("22");
        GameObject cube24 = FindCubeByName("24");
        GameObject cube10 = FindCubeByName("10");
        GameObject cube12 = FindCubeByName("12");
        GameObject cube13 = FindCubeByName("13");
        GameObject cube15 = FindCubeByName("15");

        Dictionary<GameObject, GameObject> hedefler = new Dictionary<GameObject, GameObject>
        {
            { cube18, cube10 },
            { cube20, cube12 },
            { cube22, cube13 },
            { cube24, cube15 }
        };

        // En çok eşleşenleri bul
        int max = 0;
        List<GameObject> eslesenler = new List<GameObject>();

        for (int i = 0; i < 4; i++)
        {
            List<GameObject> eslesenler_tem = GetMatchedCubes(hedefler);
            if (eslesenler_tem.Count > max)
            {
                max = eslesenler_tem.Count;
                eslesenler = new List<GameObject>(eslesenler_tem);
            }

            ust_saga(); yield return Bekle();
        }

        // Hepsi eşleşmişse
        if (max == 4)
        {
            yield return StartCoroutine(MoveCubeToTarget(eslesenler[0], hedefler[eslesenler[0]]));
            yield break;
        }

        // İkili eşleşme varsa
        if (max == 2)
        {
            float fark = Mathf.Abs(Vector3.Distance(eslesenler[0].transform.position, eslesenler[1].transform.position) - 2f);

            yield return StartCoroutine(MoveCubeToTarget(eslesenler[0], hedefler[eslesenler[0]]));

            if (fark < 0.05f) // Karşılıklıysa
            {
                while (eslesenler[0].transform.position.z != 1)
                {
                    ust_saga(); yield return Bekle();
                    orta_saga(); yield return Bekle();
                    alt_saga(); yield return Bekle();
                }
                //yield break;
                yield return StartCoroutine(hareket5_algoritma());

                ust_saga(); yield return Bekle();

                ust_saga(); yield return Bekle();
                orta_saga(); yield return Bekle();
                alt_saga(); yield return Bekle();

                yield return StartCoroutine(hareket5_algoritma());
            }
            else // Yan yana değilse
            {                
                while (!IsOneSag(eslesenler[0], eslesenler[1]))
                {
                    ust_saga(); yield return Bekle();
                    orta_saga(); yield return Bekle();
                    alt_saga(); yield return Bekle();
                }
                //yield break;
                ust_saga(); yield return Bekle();
                orta_saga(); yield return Bekle();
                alt_saga(); yield return Bekle();

                ust_saga(); yield return Bekle();
                orta_saga(); yield return Bekle();
                alt_saga(); yield return Bekle();

                yield return StartCoroutine(hareket5_algoritma());
            }
        }
    }

    // 🎯 Hedefe ulaşana kadar ilerle
    IEnumerator MoveCubeToTarget(GameObject cube, GameObject hedef)
    {
        while (Mathf.Abs(Vector3.Distance(cube.transform.position, hedef.transform.position)) - 1f >= 0.05f)
        {
            ust_saga(); yield return Bekle();
        }
    }

    // ✔️ Eşleşen küpleri bul
    List<GameObject> GetMatchedCubes(Dictionary<GameObject, GameObject> hedefler)
    {
        List<GameObject> matched = new List<GameObject>();
        foreach (var pair in hedefler)
        {
            if (Mathf.Abs(Vector3.Distance(pair.Key.transform.position, pair.Value.transform.position)) - 1f < 0.05f)
            {
                matched.Add(pair.Key);
            }
        }
        return matched;
    }


    IEnumerator hareket5_algoritma()
    {
        sag_yukari(); yield return Bekle();
        ust_sola(); yield return Bekle();
        sag_asagi(); yield return Bekle();
        ust_sola(); yield return Bekle();
        sag_yukari(); yield return Bekle();
        ust_sola(); yield return Bekle();
        ust_sola(); yield return Bekle();
        sag_asagi(); yield return Bekle();
        ust_sola(); yield return Bekle();
    }

    IEnumerator Hareket4()
    {
        List<GameObject> hazirdegillar = new List<GameObject> { };
        GameObject cube18 = FindCubeByName("18");
        GameObject cube20 = FindCubeByName("20");
        GameObject cube22 = FindCubeByName("22");
        GameObject cube24 = FindCubeByName("24");

        foreach (GameObject topmsGobj in topms_gobjs)
        {
            foreach (GameObject surface in topms)
            {
                float distance = Vector3.Distance(topmsGobj.transform.position, surface.transform.position);
                float tolerance = 0.05f;
                if (Mathf.Abs(distance - 0.5f) < tolerance)
                {
                    switch (surface.name)
                    {
                        case "24t":
                            hazirdegillar.Add(cube24);
                            break;
                        case "22t":
                            hazirdegillar.Add(cube22);
                            break;
                        case "20t":
                            hazirdegillar.Add(cube20);
                            break;
                        case "18t":
                            hazirdegillar.Add(cube18);
                            break;
                    }
                }
            }
        }

        if (hazirdegillar.Count == 0)
        {
            yield break;
        }
        else if (hazirdegillar.Count == 4)
        {
            yield return StartCoroutine(hareket4_algoritma());

            hazirdegillar.Clear();
            foreach (GameObject topmsGobj in topms_gobjs)
            {
                foreach (GameObject surface in topms)
                {
                    float distance = Vector3.Distance(topmsGobj.transform.position, surface.transform.position);
                    float tolerance = 0.05f;
                    if (Mathf.Abs(distance - 0.5f) < tolerance)
                    {
                        switch (surface.name)
                        {
                            case "24t":
                                hazirdegillar.Add(cube24);
                                break;
                            case "22t":
                                hazirdegillar.Add(cube22);
                                break;
                            case "20t":
                                hazirdegillar.Add(cube20);
                                break;
                            case "18t":
                                hazirdegillar.Add(cube18);
                                break;
                        }
                    }
                }
            }

            GameObject obj1 = hazirdegillar[0];
            GameObject obj2 = hazirdegillar[1];

            while (!IsOneSag(obj1, obj2))
            {
                ust_saga();
                yield return Bekle();
            }
            ust_saga();
            yield return Bekle();

            yield return StartCoroutine(hareket4_algoritma());
            yield return StartCoroutine(hareket4_algoritma());
        }
        else if (hazirdegillar.Count == 2)
        {
            GameObject obj1 = hazirdegillar[0];
            GameObject obj2 = hazirdegillar[1];

            float mesafe = Vector3.Distance(obj1.transform.position, obj2.transform.position);

            if (Mathf.Approximately(mesafe, 2f)) // karşılıklı
            {
                while (!(Mathf.Approximately(obj1.transform.position.x, 1f) && Mathf.Approximately(obj1.transform.position.z, 0f)))
                {
                    ust_saga();
                    yield return Bekle();
                }

                yield return StartCoroutine(hareket4_algoritma());
            }
            else // komşular
            {
                while (!IsOneSag(obj1, obj2))
                {
                    ust_saga();
                    yield return Bekle();
                }
                ust_saga();
                yield return Bekle();

                yield return StartCoroutine(hareket4_algoritma());
                yield return StartCoroutine(hareket4_algoritma());
            }
        }
    }

    List<GameObject> GetHazirDegiller(GameObject cube18, GameObject cube20, GameObject cube22, GameObject cube24)
{
    List<GameObject> hazirlarDegiller = new List<GameObject>();
    float tolerance = 1f; // derece toleransı

    Vector3 euler22 = cube22.transform.rotation.eulerAngles;
    Vector3 euler24 = cube24.transform.rotation.eulerAngles;
    Vector3 euler20 = cube20.transform.rotation.eulerAngles;
    Vector3 euler18 = cube18.transform.rotation.eulerAngles;

    // cube22 için x ve z önemli
    if (Mathf.Abs(Mathf.DeltaAngle(euler22.x, 0f)) < tolerance &&
        Mathf.Abs(Mathf.DeltaAngle(euler22.z, 270f)) < tolerance)
    {
        hazirlarDegiller.Add(cube22);
    }

    // cube24 için x ve y önemli
    if (Mathf.Abs(Mathf.DeltaAngle(euler24.x, 270f)) < tolerance &&
        Mathf.Abs(Mathf.DeltaAngle(euler24.y, 0f)) < tolerance)
    {
        hazirlarDegiller.Add(cube24);
    }

    // cube20 için x ve z önemli
    if (Mathf.Abs(Mathf.DeltaAngle(euler20.x, 0f)) < tolerance &&
        Mathf.Abs(Mathf.DeltaAngle(euler20.z, 90f)) < tolerance)
    {
        hazirlarDegiller.Add(cube20);
    }

    // cube18 için x ve y önemli
    if (Mathf.Abs(Mathf.DeltaAngle(euler18.x, 90f)) < tolerance &&
        Mathf.Abs(Mathf.DeltaAngle(euler18.y, 0f)) < tolerance)
    {
        hazirlarDegiller.Add(cube18);
    }

    return hazirlarDegiller;
}

    bool IsOneSag(GameObject a, GameObject b)
    {
        return
            (Mathf.Approximately(a.transform.position.x, 1f) && Mathf.Approximately(a.transform.position.z, 0f) &&
            Mathf.Approximately(b.transform.position.x, 0f) && Mathf.Approximately(b.transform.position.z, 1f))
            ||
            (Mathf.Approximately(a.transform.position.x, 0f) && Mathf.Approximately(a.transform.position.z, 1f) &&
            Mathf.Approximately(b.transform.position.x, 1f) && Mathf.Approximately(b.transform.position.z, 0f));
    }

    IEnumerator hareket4_algoritma()
    {
        on_saga(); yield return Bekle();
        sag_yukari(); yield return Bekle();
        ust_sola(); yield return Bekle();
        sag_asagi(); yield return Bekle();
        ust_saga(); yield return Bekle();
        on_sola(); yield return Bekle();
    }

    IEnumerator Hareket3()
    {
        GameObject cube9 = FindCubeByName("9");
        GameObject cube11 = FindCubeByName("11");
        GameObject cube14 = FindCubeByName("14");
        GameObject cube16 = FindCubeByName("16");

        GameObject cube10 = FindCubeByName("10");
        GameObject cube12 = FindCubeByName("12");
        GameObject cube13 = FindCubeByName("13");
        GameObject cube15 = FindCubeByName("15");

        yield return StartCoroutine(KupuIsle(cube9, cube12, cube10, 90f, false));
        yield return StartCoroutine(KupuIsle(cube11, cube13, cube10, 90f, true));
        yield return StartCoroutine(KupuIsle(cube14, cube12, cube15, 270f, true));
        yield return StartCoroutine(KupuIsle(cube16, cube15, cube13, 0f, true));
    }

    IEnumerator KupuIsle(GameObject hedefKüp, GameObject kontrolKüp1, GameObject kontrolKüp2, float aci, bool sola)
    {
        if (hedefKüp.transform.position.y == 0) // Küp ortadaysa yukarı çıkar
        {
            while (hedefKüp.transform.position.x != 1 || hedefKüp.transform.position.z != 1)
            {
                orta_saga(); yield return Bekle();
                alt_saga(); yield return Bekle();
            }
            yield return StartCoroutine(hareket3_sola_tasi());
        }

        while (hedefKüp.transform.position.x != 0 || hedefKüp.transform.position.z != 1)
        {
            ust_saga(); yield return Bekle();
        }

        if (Mathf.Approximately(hedefKüp.transform.eulerAngles.x, aci))
        {
            while (kontrolKüp1.transform.position.x != 0 || kontrolKüp1.transform.position.z != 1)
            {
                orta_saga(); yield return Bekle();
                alt_saga(); yield return Bekle();
            }
            yield return StartCoroutine(sola ? hareket3_sola_tasi() : hareket3_saga_tasi());
        }
        else
        {
            while (kontrolKüp2.transform.position.x != 0 || kontrolKüp2.transform.position.z != 1)
            {
                orta_saga(); yield return Bekle();
                alt_saga(); yield return Bekle();
            }
            yield return StartCoroutine(sola ? hareket3_saga_tasi() : hareket3_sola_tasi());
        }
    }

    IEnumerator hareket3_sola_tasi()
    {
        ust_saga(); yield return Bekle();
        sol_yukari(); yield return Bekle();
        ust_sola(); yield return Bekle();
        sol_asagi(); yield return Bekle();
        ust_sola(); yield return Bekle();
        on_saga(); yield return Bekle();
        ust_saga(); yield return Bekle();
        on_sola(); yield return Bekle();
    }

    IEnumerator hareket3_saga_tasi()
    {
        ust_sola(); yield return Bekle();
        sag_yukari(); yield return Bekle();
        ust_saga(); yield return Bekle();
        sag_asagi(); yield return Bekle();
        ust_saga(); yield return Bekle();
        on_sola(); yield return Bekle();
        ust_sola(); yield return Bekle();
        on_saga(); yield return Bekle();
    }

    IEnumerator Hareket2()
    {
        GameObject cube0 = FindCubeByName("0");
        GameObject cube2 = FindCubeByName("2");
        GameObject cube6 = FindCubeByName("6");
        GameObject cube8 = FindCubeByName("8");

        GameObject cube1 = FindCubeByName("1");
        GameObject cube3 = FindCubeByName("3");
        GameObject cube5 = FindCubeByName("5");
        GameObject cube7 = FindCubeByName("7");

        yield return StartCoroutine(Hizala(cube0, cube3, cube1));
        yield return StartCoroutine(Hizala(cube2, cube1, cube5));
        yield return StartCoroutine(Hizala(cube6, cube7, cube3));
        yield return StartCoroutine(Hizala(cube8, cube5, cube7));
    }

    IEnumerator Hizala(GameObject hedefKüp, GameObject komsu1, GameObject komsu2)
    {
        if (hedefKüp.transform.position.y == 1 &&
            hedefKüp.transform.rotation.x == 0 && hedefKüp.transform.rotation.z == 0 &&
            Mathf.Abs(Vector3.Distance(hedefKüp.transform.position, komsu1.transform.position) - 1f) < 0.001f &&
            Mathf.Abs(Vector3.Distance(hedefKüp.transform.position, komsu2.transform.position) - 1f) < 0.001f)
        {
            yield break;
        }

        if (hedefKüp.transform.position.y == 1) // hedef küp yukardaysa en alt satıra taşı
        {
            while (hedefKüp.transform.position.x != -1 || hedefKüp.transform.position.z != 1)
                {
                    ust_saga(); yield return Bekle();
                    orta_saga(); yield return Bekle();
                }

            sag_asagi(); yield return Bekle();
            alt_sola(); yield return Bekle();
            sag_yukari(); yield return Bekle();
            alt_saga(); yield return Bekle();
        }

        while (hedefKüp.transform.position.x != -1 || hedefKüp.transform.position.z != 1) // hedef kübü alt sağa taşı
        {
            alt_saga(); yield return Bekle();
        }

        // Hedef konuma gelecek komşuyu hizaya getir
        while (komsu1.transform.position.x != -1 || komsu1.transform.position.z != 0)
        {
            ust_saga(); yield return Bekle();
            orta_saga(); yield return Bekle();
        }

        while (hedefKüp.transform.position.x != -1 || hedefKüp.transform.position.z != 1 ||
                hedefKüp.transform.position.y != 1 ||
                hedefKüp.transform.rotation.x != 0 || hedefKüp.transform.rotation.z != 0)
        {
            sag_asagi(); yield return Bekle();
            alt_sola(); yield return Bekle();
            sag_yukari(); yield return Bekle();
            alt_saga(); yield return Bekle();
        }
    }

    IEnumerator Hareket1()
    {
        bool hiza1 = false, hiza2 = false, hiza3 = false, hiza4 = false;

        GameObject cube15 = FindCubeByName("15");
        GameObject cube7 = FindCubeByName("7");
        GameObject cube13 = FindCubeByName("13");
        GameObject cube5 = FindCubeByName("5");
        GameObject cube10 = FindCubeByName("10");
        GameObject cube1 = FindCubeByName("1");
        GameObject cube12 = FindCubeByName("12");
        GameObject cube3 = FindCubeByName("3");

        int max = 0;
        for (int i = 0; i < 4; i++)
        {
            int j = 0;
            if (KupleHizaliMi(cube13, cube5)) { j++; }
            if (KupleHizaliMi(cube10, cube1)) { j++; }
            if (KupleHizaliMi(cube15, cube7)) { j++; }
            if (KupleHizaliMi(cube12, cube3)) { j++; }

            if (j > max)
            {
                max = j;
            }

            orta_saga(); yield return Bekle();
        }

        if (max == 4)
        {
            while (cube7.transform.position.x != cube15.transform.position.x)
            {
                orta_saga(); yield return Bekle();
            }
        }

        if (max == 2)
        {
            int i = 0;
            while (i < 2)
            {
                i = 0;
                hiza1 = hiza2 = hiza3 = hiza4 = false;

                orta_saga(); yield return Bekle();

                if (KupleHizaliMi(cube13, cube5)) { i++; hiza1 = true; }
                if (KupleHizaliMi(cube10, cube1)) { i++; hiza2 = true; }
                if (KupleHizaliMi(cube15, cube7)) { i++; hiza3 = true; }
                if (KupleHizaliMi(cube12, cube3)) { i++; hiza4 = true; }
            }

            //karşılıklıysa
            if ((hiza2 && hiza3) || (hiza1 && hiza4))
            {
                if (hiza2 && hiza3)
                {
                    while (cube15.transform.position.z == 1)
                    {
                        ust_saga(); yield return Bekle();
                        orta_saga(); yield return Bekle();
                    }
                }
                if (hiza1 && hiza4)
                {
                    while (cube13.transform.position.z == 1)
                    {
                        ust_saga(); yield return Bekle();
                        orta_saga(); yield return Bekle();
                    }
                }
                yield return NihaiHareket();
                ust_saga(); yield return Bekle();
                yield return NihaiHareket();
            }
            else // komşuysa
            {
                i = 0;
                while (i < 2)
                {
                    i = 0;
                    hiza1 = hiza2 = hiza3 = hiza4 = false;

                    orta_saga(); yield return Bekle();

                    if (KupleHizaliMi(cube13, cube5)) { i++; hiza1 = true; }
                    if (KupleHizaliMi(cube10, cube1)) { i++; hiza2 = true; }
                    if (KupleHizaliMi(cube15, cube7)) { i++; hiza3 = true; }
                    if (KupleHizaliMi(cube12, cube3)) { i++; hiza4 = true; }
                }

                if (hiza1 && hiza3) yield return HizaAyari(cube5);
                if (hiza3 && hiza4) yield return HizaAyari(cube7);
                if (hiza2 && hiza4) yield return HizaAyari(cube3);
                if (hiza1 && hiza2) yield return HizaAyari(cube1);

                // nihai algoritma
                yield return NihaiHareket();
            }
        }
    }

    bool KupleHizaliMi(GameObject alt, GameObject ust)
    {
        return ust != null && alt != null &&
            ust.transform.position.x == alt.transform.position.x &&
            ust.transform.position.z == alt.transform.position.z &&
            ust.transform.position.y == alt.transform.position.y + 1;
    }

    IEnumerator HizaAyari(GameObject hedefCube)
    {
        while (hedefCube.transform.position.x != 1)
        {
            ust_saga(); yield return Bekle();
            orta_saga(); yield return Bekle();
            alt_saga(); yield return Bekle();
        }
    }

    IEnumerator NihaiHareket()
    {
        sag_yukari(); yield return Bekle();
        ust_saga(); yield return Bekle();
        sag_asagi(); yield return Bekle();
        ust_sola(); yield return Bekle();
        sag_yukari(); yield return Bekle();
    }

    WaitForSeconds Bekle()
    {
        return new WaitForSeconds(0.1f);
    }

    private IEnumerator RotateSequence()
    {
        for (int i = 0; i < 15; i++)
        {
            int randomIndex = Random.Range(0, 18); // 18 fonksiyon var
            switch (randomIndex)
            {
                case 0: ust_sola(); break;
                case 1: ust_saga(); break;
                case 2: orta_sola(); break;
                case 3: orta_saga(); break;
                case 4: alt_sola(); break;
                case 5: alt_saga(); break;
                case 6: sol_yukari(); break;
                case 7: sol_asagi(); break;
                case 8: orta_yukari(); break;
                case 9: orta_asagi(); break;
                case 10: sag_yukari(); break;
                case 11: sag_asagi(); break;
                case 12: on_sola(); break;
                case 13: on_saga(); break;
                case 14: der_orta_sola(); break;
                case 15: der_orta_saga(); break;
                case 16: arka_sola(); break;
                case 17: arka_saga(); break;
            }

            // Bekleme süresi: her dönüş arasında biraz zaman tanı
            yield return new WaitForSeconds(0.1f); // istersen bu süreyi azalt veya artır
        }
    }

    // yatay eksende hareketler
    public void ust_sola() { TriggerRotation("y", 1f, 90f, 3); ReorderCubes(0,1,2,3,4,5,6,7,8,false); }
    public void ust_saga() { TriggerRotation("y", 1f, -90f, 3); ReorderCubes(0,1,2,3,4,5,6,7,8,true); }

    public void orta_sola() { TriggerRotation("y", 0f, 90f, 2); ReorderCubes(9,10,11,12,-1,13,14,15,16,false); }
    public void orta_saga() { TriggerRotation("y", 0f, -90f, 2); ReorderCubes(9,10,11,12,-1,13,14,15,16,true); }

    public void alt_sola() { TriggerRotation("y", -1f, 90f, 4); ReorderCubes(17,18,19,20,21,22,23,24,25,false); }
    public void alt_saga() { TriggerRotation("y", -1f, -90f, 4); ReorderCubes(17,18,19,20,21,22,23,24,25,true); }

    // dikey eksende hareketler
    public void sol_yukari() { TriggerRotation("x", 1f, -90f, 6); ReorderCubes(6,3,0,14,12,9,23,20,17,false); }
    public void sol_asagi() { TriggerRotation("x", 1f, 90f, 6); ReorderCubes(6,3,0,14,12,9,23,20,17,true); }

    public void orta_yukari() { TriggerRotation("x", 0f, -90f, 2); ReorderCubes(7,4,1,15,-1,10,24,21,18,false); }
    public void orta_asagi() { TriggerRotation("x", 0f, 90f, 2); ReorderCubes(7,4,1,15,-1,10,24,21,18,true); }

    public void sag_yukari() { TriggerRotation("x", -1f, -90f, 5); ReorderCubes(8,5,2,16,13,11,25,22,19,false); }
    public void sag_asagi() { TriggerRotation("x", -1f, 90f, 5); ReorderCubes(8,5,2,16,13,11,25,22,19,true); }

    // derinlik ekseninde hareketler
    public void on_sola() { TriggerRotation("z", 1f, -90f, 0); ReorderCubes(6,7,8,14,15,16,23,24,25,true); }
    public void on_saga() { TriggerRotation("z", 1f, 90f, 0); ReorderCubes(6,7,8,14,15,16,23,24,25,false); }

    public void der_orta_sola() { TriggerRotation("z", 0f, -90f, 2); ReorderCubes(3,4,5,12,-1,13,20,21,22,true); }
    public void der_orta_saga() { TriggerRotation("z", 0f, 90f, 2); ReorderCubes(3,4,5,12,-1,13,20,21,22,false); }

    public void arka_sola() { TriggerRotation("z", -1f, -90f, 1); ReorderCubes(0,1,2,9,10,11,17,18,19,true); }
    public void arka_saga() { TriggerRotation("z", -1f, 90f, 1); ReorderCubes(0,1,2,9,10,11,17,18,19,false); }

    public void TriggerRotation(string eksen, float konum, float miktar, int merkez)
    {
        StartCoroutine(RotateCubes(eksen, konum, miktar, merkez));
    }

    IEnumerator RotateCubes(string eksen, float konum, float miktar, int merkez)
    {
        List<GameObject> filtered = new List<GameObject> { };
        if (eksen == "x")
        {
            filtered = cubes
            .Where(cube => Mathf.Approximately(cube.transform.position.x, konum))
            .ToList();
        }
        if (eksen == "y")
        {
            filtered = cubes
            .Where(cube => Mathf.Approximately(cube.transform.position.y, konum))
            .ToList();
        }
        if (eksen == "z")
        {
            filtered = cubes
            .Where(cube => Mathf.Approximately(cube.transform.position.z, konum))
            .ToList();
        }

        GameObject targetCenter = centers[merkez];

        foreach (GameObject cube in filtered)
        {
            cube.transform.SetParent(targetCenter.transform);
        }

        yield return StartCoroutine(Rotate(targetCenter, eksen, miktar));

        foreach (GameObject cube in filtered)
        {
            cube.transform.SetParent(null);
        }

        foreach (GameObject cube in filtered)
        {
            Vector3 pos = cube.transform.position;

            pos.x = GetNearest(pos.x);
            pos.y = GetNearest(pos.y);
            pos.z = GetNearest(pos.z);

            cube.transform.position = pos;

            targetCenter.transform.rotation = Quaternion.identity;
        }

        targetCenter.transform.rotation = Quaternion.identity;

    }

    IEnumerator Rotate(GameObject target, string rotate, float amount)
    {
        Quaternion startRotation = target.transform.rotation;
        Quaternion deltaRotation = Quaternion.identity;

        if (rotate == "x")
        {
            deltaRotation = Quaternion.AngleAxis(amount, Vector3.right);
        }
        else if (rotate == "y")
        {
            deltaRotation = Quaternion.AngleAxis(amount, Vector3.up);
        }
        else if (rotate == "z")
        {
            deltaRotation = Quaternion.AngleAxis(amount, Vector3.forward);
        }

        Quaternion endRotation = startRotation * deltaRotation;

        float duration = 0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            target.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.transform.rotation = endRotation;
    }

    float GetNearestAngle(float angle)
    {
        float[] options = { 0f, 90f, 180f, 270f, 360f };
        float closest = options[0];
        float minDist = Mathf.Abs(Mathf.DeltaAngle(angle, options[0]));

        for (int i = 1; i < options.Length; i++)
        {
            float dist = Mathf.Abs(Mathf.DeltaAngle(angle, options[i]));
            if (dist < minDist)
            {
                minDist = dist;
                closest = options[i];
            }
        }
        return closest % 360f;
    }

    float GetNearest(float value)
    {
        float[] options = { -1f, 0f, 1f };
        float closest = options[0];
        float minDist = Mathf.Abs(value - options[0]);

        for (int i = 1; i < options.Length; i++)
        {
            float dist = Mathf.Abs(value - options[i]);
            if (dist < minDist)
            {
                minDist = dist;
                closest = options[i];
            }
        }
        return closest;
    }

    public void ReorderCubes(int i0, int i1, int i2, int i3, int i4, int i5, int i6, int i7, int i8, bool drc /*true ise sağa*/)
    {
        int[] indices = { i0, i1, i2, i3, i4, i5, i6, i7, i8 };
        int[] yeni = new int[9];
        int[,] matris = new int[3, 3];

        // Matrise çevir
        for (int i = 0; i < 9; i++)
        {
            matris[i / 3, i % 3] = indices[i];
        }

        if (!drc)
        {
            // matrisi sola döndürme gibi davranır
            int index = 0;
            for (int col = 2; col >= 0; col--)
            {
                for (int row = 0; row < 3; row++)
                {
                    yeni[index++] = matris[row, col];
                }
            }
        }
        else
        {
            // matrisi sağa döndürme gibi davranır
            int index = 0;
            for (int col = 0; col < 3; col++)
            {
                for (int row = 2; row >= 0; row--)
                {
                    yeni[index++] = matris[row, col];
                }
            }
        }

        // Geçici cube listesi oluştur (veri kaybını engellemek için)
        List<GameObject> temp = new List<GameObject>();
        for (int i = 8; i >= 0; i--)
        {
            if (yeni[i] >= 0)
            {
                temp.Add(cubes[yeni[i]]);
            }
            else
            {
                temp.Add(null);
            }
        }

        // cubes listesini yeni sıraya göre güncelle
        for (int i = 8; i >= 0; i--)
        {
            if (indices[i] >= 0)
            {
                cubes[indices[i]] = temp[i];
            }
        }
    }

}
