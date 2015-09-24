using UnityEngine;
using System;
using System.Collections;
using System.Security.Cryptography;

public class CommonUtil {

    /// <summary>
    /// プレハブインスタンス
    /// </summary>
    /// <param name="objName">オブジェクト名</param>
    /// <param name="objPath">オブジェクトパス</param>
    /// <param name="parent">親オブジェクト</param>
    /// <returns>GameObject</returns>
    public static GameObject PrefabInstance(string objName, string objPath, Transform parent)
    {
        GameObject obj = PrefabInstance(objPath, parent);
        obj.name = objName;
        return obj;
    }

    /// <summary>
    /// プレハブインスタンス
    /// </summary>
    /// <param name="objPath">オブジェクトパス</param>
    /// <param name="parent">親Transform</param>
    /// <returns>GameObject</returns>
    public static GameObject PrefabInstance(string objPath, Transform parent = null)
    {
        if (objPath == null) {
            return null;
        }
        GameObject obj = GameObject.Instantiate(Resources.Load(objPath)) as GameObject;
        if (obj != null && parent != null) {
            obj.transform.parent = parent;
        }
        return obj;
    }

    /// <summary>
    /// プレハブインスタンス
    /// </summary>
    /// <param name="prefab">プレハブ</param>
    /// <param name="parent">親Transform</param>
    /// <returns>GameObject</returns>
    public static GameObject PrefabInstance(GameObject prefab, Transform parent = null) {
        if (prefab == null) { return null; }
        GameObject obj = GameObject.Instantiate(prefab) as GameObject;
        if (obj != null) {
            obj.name = prefab.name;
            if (parent != null) {
                obj.transform.parent = parent;
            }
        }
        return obj;
    }

    /// <summary>
    /// オブジェクトを検索する
    /// </summary>
    /// <param name="objectName">探したいオブジェクト名</param>
    /// <param name="trans">探したいオブジェクトが存在するTransform</param>
    /// <param name="count">探索個数</param>
    /// <param name="countNum">何番目を引っ掛けるか</param>
    /// <returns></returns>
    public static GameObject SearchObject(string objectName, Transform trans, int count, int countNum){
        if (trans.gameObject.name.Equals(objectName)) {
            if (count >= countNum) {
                return trans.gameObject;
            }
            count++;
        }
        foreach (Transform child in trans) {
            GameObject obj = SearchObject(objectName, child, count, countNum);
            if (obj != null) {
                return obj;
            }
        }
        return null;
    }


    /// <summary>
    /// 子オブジェクトを探索する
    /// </summary>
    /// <param name="objectName">探したいオブジェクト名</param>
    /// <param name="parent">探索したいオブジェクトが存在するTransform</param>
    /// <param name="countNum">何番目を引っ掛けるか</param>
	public static GameObject SearchObjectChild(string objectName, Transform parent = null, int countNum = 0){
        int count = 0;
        if(parent == null){
            return GameObject.Find(objectName);
        }
        foreach (Transform child in parent) {
            GameObject obj = SearchObject(objectName, child, count, countNum);
            if (obj != null) {
                return obj;
            }
        }
        return null;
    }

    /// <summary>
    /// GameObjectにTextureをセット
    /// </summary>
    /// <param name="texture">Texture2D</param>
    /// <param name="objName">オブジェクト名</param>
    /// <returns></returns>
    public static GameObject SetTexture(Texture2D texture, string objName) {
        GameObject obj = SearchObjectChild(objName);
        if (obj == null) {
            obj = PrefabInstance(objName, "Prefabs/Common/" + objName, null);
            if (obj == null) {
                obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = new Vector3(0, 0, 0);
                obj.transform.localScale = new Vector3(1024, 768, 0);
            }
        }
        return obj;
    }

    /// <summary>
    /// GameObjectにTextureをセット
    /// </summary>
    /// <param name="texture">Texture2D</param>
    /// <param name="obj">オブジェクト名</param>
    public static void SetTexture(Texture2D texture, GameObject obj) {
        if (obj == null) { return; }
        obj.renderer.material.mainTexture = texture;
    }

    /// <summary>
    /// ランダムで抽選した値をひとつ返す
    /// </summary>
    /// <typeparam name="T">使用する型</typeparam>
    /// <param name="value">使用する配列</param>
    /// <returns>配列の中の値の一つ</returns>
    public static T RandPickOne<T>(T[] value) {
        return value[CreateRandomNumber(value.Length)];
    }

    /// <summary>
    /// ランダムで抽選した値の配列を返す
    /// </summary>
    /// <typeparam name="T">使用する型</typeparam>
    /// <param name="value">使用する配列</param>
    /// <param name="len">戻す長さ</param>
    /// <returns>指定長の配列</returns>
    public static T[] RandPickUp<T>(T[] value, int len) {
        // 型の配列を指定長で生成
        T[] array = new T[len];
        for (int ii = 0; ii < len; ii++) {
            array[ii] = RandPickOne<T>(value);
        }
        return array;
    }

    /// <summary>
    /// ランダム数値を作成
    /// </summary>
    /// <param name="max">ランダムの最大値</param>
    /// <returns>ランダム値</returns>
    public static int CreateRandomNumber(int i_max) {
        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        int max = i_max;
        // BitConverterの仕様で４バイトの長さが必要なので4より最大が4より小さい場合は強制4にしてしまう
        if (max < 4) { max = 4; }
        byte[] rand = new byte[max];
        rng.GetBytes(rand);
        // 配列の最大長で乱数を生成
        int value = 1 + (int)((max - 1) * (BitConverter.ToUInt32(rand, 0) / ((double)uint.MaxValue + 1.0)));
        
        // 最大を強制4にしている影響で最大を超える場合が存在するので、そういう場合はUnity既存のRandomに頼る
        if (value > i_max) { value = UnityEngine.Random.Range(0, i_max); }
        return value;
    }

    /// <summary>
    /// 最低値と最大値の間からランダム数値を作成
    /// </summary>
    /// <param name="min">ランダムの最低値</param>
    /// <param name="max">ランダムの最大値</param>
    /// <returns>ランダム値</returns>
    public static int CreateRandomNumber(int min, int max) {
        int value = 0;
        // 同値だったら最低値を返す
        if (min == max) { return min; }
        if (min > max)  { return max; }

        // BitConverterの仕様で４バイトの長さが必要なので4より最大が4より小さい場合は強制4にしてしまう
        if (max < 4) { max = 4; }
        do {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] rand = new byte[max];
            rng.GetBytes(rand);
            // 配列の最大長で乱数を生成
            value = 1 + (int)((max - 1) * (BitConverter.ToUInt32(rand, 0) / ((double)uint.MaxValue + 1.0)));
        } while (value < min);

        // 最大を強制4にしている影響で最大を超える場合が存在するので、そういう場合はUnity既存のRandomに頼る
        if (value > max) { value = UnityEngine.Random.Range(min, max); }
        return value;
    }

    /// <summary>
    /// ランダムでTrueかFalseを返す
    /// </summary>
    /// <returns>bool</returns>
    public static bool GetRandomTorF() {
        return CreateRandomNumber(0, CreateRandomNumber(5)) == CreateRandomNumber(5);
    }

    /// <summary>
    /// 文字列をcharバイトコードに変換
    /// </summary>
    /// <param name="str">文字列</param>
    /// <returns>バイトコード配列</returns>
    public static byte[] GetStrToByte(string str) {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

    /// <summary>
    /// バイトコードを文字列に変換
    /// </summary>
    /// <param name="bytes">バイトコード配列</param>
    /// <returns>文字列</returns>
    public static string GetByteToStr(byte[] bytes) {
        char[] chars = new char[bytes.Length / sizeof(char)];
        Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }

    /// <summary>
    /// 文字列バイトコードから文字コードを判定する
    /// </summary>
    /// <param name="bytes">バイトデータ</param>
    /// <returns>文字コード</returns>
    public static System.Text.Encoding CheckBytesCode(byte[] bytes) {
        const byte bEscape = 0x1B;
        const byte bAt = 0x40;
        const byte bDollar = 0x24;
        const byte bAnd = 0x26;
        const byte bOpen = 0x28;    //'('
        const byte bB = 0x42;
        const byte bD = 0x44;
        const byte bJ = 0x4A;
        const byte bI = 0x49;

        int len = bytes.Length;
        byte b1, b2, b3, b4;

        for (int i = 0; i < len - 2; i++) {
            b1 = bytes[i];
            b2 = bytes[i + 1];
            b3 = bytes[i + 2];

            if (b1 == bEscape) {
                if (b2 == bDollar && b3 == bAt) {
                    //JIS_0208 1978
                    //JIS
                    return System.Text.Encoding.GetEncoding(50220);
                }
                else if (b2 == bDollar && b3 == bB) {
                    //JIS_0208 1983
                    //JIS
                    return System.Text.Encoding.GetEncoding(50220);
                }
                else if (b2 == bOpen && (b3 == bB || b3 == bJ)) {
                    //JIS_ASC
                    //JIS
                    return System.Text.Encoding.GetEncoding(50220);
                }
                else if (b2 == bOpen && b3 == bI) {
                    //JIS_KANA
                    //JIS
                    return System.Text.Encoding.GetEncoding(50220);
                }
                if (i < len - 3) {
                    b4 = bytes[i + 3];
                    if (b2 == bDollar && b3 == bOpen && b4 == bD) {
                        //JIS_0212
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    if (i < len - 5 &&
                        b2 == bAnd && b3 == bAt && b4 == bEscape &&
                        bytes[i + 4] == bDollar && bytes[i + 5] == bB) {
                        //JIS_0208 1990
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                }
            }
        }

        int sjis = 0;
        int euc = 0;
        int utf8 = 0;
        for (int i = 0; i < len - 1; i++) {
            b1 = bytes[i];
            b2 = bytes[i + 1];
            if (((0x81 <= b1 && b1 <= 0x9F) || (0xE0 <= b1 && b1 <= 0xFC)) &&
                ((0x40 <= b2 && b2 <= 0x7E) || (0x80 <= b2 && b2 <= 0xFC))) {
                //SJIS_C
                sjis += 2;
                i++;
            }
        }
        for (int i = 0; i < len - 1; i++) {
            b1 = bytes[i];
            b2 = bytes[i + 1];
            if (((0xA1 <= b1 && b1 <= 0xFE) && (0xA1 <= b2 && b2 <= 0xFE)) ||
                (b1 == 0x8E && (0xA1 <= b2 && b2 <= 0xDF))) {
                //EUC_C
                //EUC_KANA
                euc += 2;
                i++;
            }
            else if (i < len - 2) {
                b3 = bytes[i + 2];
                if (b1 == 0x8F && (0xA1 <= b2 && b2 <= 0xFE) &&
                    (0xA1 <= b3 && b3 <= 0xFE)) {
                    //EUC_0212
                    euc += 3;
                    i += 2;
                }
            }
        }
        for (int i = 0; i < len - 1; i++) {
            b1 = bytes[i];
            b2 = bytes[i + 1];
            if ((0xC0 <= b1 && b1 <= 0xDF) && (0x80 <= b2 && b2 <= 0xBF)) {
                //UTF8
                utf8 += 2;
                i++;
            }
            else if (i < len - 2) {
                b3 = bytes[i + 2];
                if ((0xE0 <= b1 && b1 <= 0xEF) && (0x80 <= b2 && b2 <= 0xBF) &&
                    (0x80 <= b3 && b3 <= 0xBF)) {
                    //UTF8
                    utf8 += 3;
                    i += 2;
                }
            }
        }

        System.Diagnostics.Debug.WriteLine(
            string.Format("sjis = {0}, euc = {1}, utf8 = {2}", sjis, euc, utf8));
        if (euc > sjis && euc > utf8) {
            //EUC
            return System.Text.Encoding.GetEncoding(51932);
        }
        else if (sjis > euc && sjis > utf8) {
            //SJIS
            return System.Text.Encoding.GetEncoding(932);
        }
        else if (utf8 > euc && utf8 > sjis) {
            //UTF8
            return System.Text.Encoding.UTF8;
        }

        return null;
    }

    /// <summary>
    /// shift-JIS文字列をUTF-8文字列に変換
    /// </summary>
    /// <param name="shiftStrings"></param>
    /// <returns></returns>
    private string ConvertShiftJIStoUTF8(string shiftStrings) {
        System.Text.Encoding shiftJIS = System.Text.Encoding.GetEncoding("shift_jis");
        byte[] shiftBytes = shiftJIS.GetBytes(shiftStrings);

        System.Text.Encoding utf = System.Text.Encoding.UTF8;
        byte[] convStringData = System.Text.Encoding.Convert(shiftJIS, utf, shiftBytes);
        char[] convCharData = new char[utf.GetCharCount(convStringData, 0, convStringData.Length)];

        utf.GetChars(convStringData, 0, convStringData.Length, convCharData, 0);

        return new string(convCharData);
    }
}
