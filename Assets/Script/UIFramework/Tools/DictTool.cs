/**
 * Created by zhang on 2019/1/26
 */
using System.Collections.Generic;
//工具类
public static class DictTool
{
    //Dictionnary的拓展
    public static Tvalue GetValue<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key)
    {
        Tvalue value = default(Tvalue);
        dict.TryGetValue(key, out value);
        return value;
    }
}