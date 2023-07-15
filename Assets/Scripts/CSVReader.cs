using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// https://github.com/salt26/chordingcoding/blob/master/ChordingCoding/Util.cs
public class CSVReader
{
    StreamReader streamReader;
    List<List<string>> data = new List<List<string>>();
    List<string> header = new List<string>();
    bool hasHeader = false;
    Dictionary<int, List<string>> columns = new Dictionary<int, List<string>>();

    /// <summary>
    /// filename의 .csv 파일을 읽고 그 데이터를 정리하여 보관합니다.
    /// </summary>
    /// <param name="filename">파일 이름(경로)</param>
    /// <param name="hasHeader">첫 줄에 헤더가 오면 true, 헤더 없이 데이터가 바로 오면 false</param>
    /// <param name="delimiter">열 구분 문자</param>
    public CSVReader(string filename, bool hasHeader, char delimiter = ',')
    {
        streamReader = new StreamReader(filename, Encoding.GetEncoding("UTF-8"));
        int i = 0;
        if (hasHeader)
        {
            i = -1;
            this.hasHeader = true;
        }
        while (!streamReader.EndOfStream)
        {
            string s = streamReader.ReadLine();
            if (i >= 0)
            {
                data.Add(new List<string>());
                string[] temp = s.Split(delimiter);
                for (int j = 0; j < temp.Length; j++)
                {
                    data[i].Add(temp[j]);
                }
            }
            else
            {
                string[] temp = s.Split(delimiter);
                for (int j = 0; j < temp.Length; j++)
                {
                    header.Add(temp[j]);
                }
            }
            i++;
        }
    }

    /// <summary>
    /// Unity TextAsset으로 된 csvFile을 읽고 그 데이터를 정리하여 보관합니다.
    /// </summary>
    /// <param name="filename">파일 이름(경로)</param>
    /// <param name="hasHeader">첫 줄에 헤더가 오면 true, 헤더 없이 데이터가 바로 오면 false</param>
    /// <param name="delimiter">열 구분 문자</param>
    public CSVReader(TextAsset csvFile, bool hasHeader, char delimiter = ',')
    {
        int i = 0;
        if (hasHeader)
        {
            i = -1;
            this.hasHeader = true;
        }
        string[] lines = csvFile.text.Split('\n');
        foreach (string s in lines)
        {
            if (i >= 0)
            {
                data.Add(new List<string>());
                string[] temp = s.Split(delimiter);
                for (int j = 0; j < temp.Length; j++)
                {
                    data[i].Add(temp[j]);
                }
            }
            else
            {
                string[] temp = s.Split(delimiter);
                for (int j = 0; j < temp.Length; j++)
                {
                    header.Add(temp[j]);
                }
            }
            i++;
        }
    }

    /// <summary>
    /// 주어진 headerName이 몇 번째 열(0부터 시작)의 헤더 이름인지 반환합니다.
    /// 없으면 -1을 반환합니다.
    /// </summary>
    /// <param name="headerName">헤더 이름</param>
    /// <returns></returns>
    public int GetHeaderIndex(string headerName)
    {
        return header.IndexOf(headerName);
    }

    /// <summary>
    /// index(0부터 시작)번째 행의 데이터를 반환합니다.
    /// 반환된 데이터를 함부로 수정하지 마세요.
    /// </summary>
    /// <param name="index">행 인덱스 (0 이상)</param>
    /// <returns></returns>
    public List<string> GetRow(int index)
    {
        if (index < 0 || index >= data.Count) return null;
        return data[index];
    }

    /// <summary>
    /// headerIndex(0부터 시작)번째 열의 데이터를 반환합니다.
    /// 특정 headerIndex에 대해 처음 호출하면 시간이 조금 걸릴 수 있습니다.
    /// 데이터의 어떤 행에 headerIndex번째 값이 없으면
    /// 그 행의 값은 null로 채워집니다.
    /// </summary>
    /// <param name="headerIndex">열 인덱스 (0 이상)</param>
    /// <returns></returns>
    public List<string> GetColumn(int headerIndex)
    {
        if (headerIndex < 0)
        {
            return null;
        }
        else if (columns.ContainsKey(headerIndex))
        {
            return columns[headerIndex];
        }
        else
        {
            List<string> column = new List<string>();
            for (int i = 0; i < data.Count; i++)
            {
                if (headerIndex < data[i].Count)
                {
                    column.Add(data[i][headerIndex]);
                }
                else
                {
                    column.Add(null);
                }
            }
            columns.Add(headerIndex, column);
            return column;
        }
    }

    /// <summary>
    /// headerName을 헤더 이름으로 하는 열의 데이터를 반환합니다.
    /// 특정 headerName에 대해 처음 호출하면 시간이 조금 걸릴 수 있습니다.
    /// </summary>
    /// <param name="headerName">헤더 이름</param>
    /// <returns></returns>
    public List<string> GetColumn(string headerName)
    {
        return GetColumn(GetHeaderIndex(headerName));
    }

    /// <summary>
    /// 헤더가 있는 경우 헤더 목록을 반환합니다.
    /// 없으면 null을 반환합니다.
    /// </summary>
    /// <returns></returns>
    public List<string> GetHeader()
    {
        if (hasHeader)
            return header;
        else return null;
    }

    /// <summary>
    /// 데이터 전체를 반환합니다.
    /// 반환된 이중 배열의 첫 번째 인덱스는 행 인덱스,
    /// 두 번째 인덱스는 열 인덱스입니다.
    /// 반환된 데이터를 함부로 수정하지 마세요.
    /// </summary>
    /// <returns></returns>
    public List<List<string>> GetData()
    {
        return data;
    }
}
