// Copyright (c) Pomelo Foundation. All rights reserved.
// Licensed under the MIT. See LICENSE in the project root for license information.

using System;
using System.Data;
using Pomelo.Data.MySql;
using System.Globalization;

namespace Pomelo.Data.Types
{
  internal struct MySqlByte : IMySqlValue
  {
    private sbyte mValue;
    private bool isNull;
    private bool treatAsBool;

    public MySqlByte(bool isNull)
    {
      this.isNull = isNull;
      mValue = 0;
      treatAsBool = false;
    }

    public MySqlByte(sbyte val)
    {
      this.isNull = false;
      mValue = val;
      treatAsBool = false;
    }

    #region IMySqlValue Members

    public bool IsNull
    {
      get { return isNull; }
    }

    MySqlDbType IMySqlValue.MySqlDbType
    {
      get { return MySqlDbType.Byte; }
    }

    object IMySqlValue.Value
    {
      get
      {
        if (TreatAsBoolean)
          return Convert.ToBoolean(mValue);
        return mValue;
      }
    }

    public sbyte Value
    {
      get { return mValue; }
      set { mValue = value; }
    }

    Type IMySqlValue.SystemType
    {
      get
      {
        if (TreatAsBoolean)
          return typeof(Boolean);
        return typeof(sbyte);
      }
    }

    string IMySqlValue.MySqlTypeName
    {
      get { return "TINYINT"; }
    }

    void IMySqlValue.WriteValue(MySqlPacket packet, bool binary, object val, int length)
    {
      sbyte v = (val is sbyte) ? (sbyte)val : Convert.ToSByte(val);
      if (binary)
        packet.WriteByte((byte)v);
      else
        packet.WriteStringNoNull(v.ToString());
    }

    IMySqlValue IMySqlValue.ReadValue(MySqlPacket packet, long length, bool nullVal)
    {
      if (nullVal)
        return new MySqlByte(true);

      if (length == -1)
        return new MySqlByte((sbyte)packet.ReadByte());
      else
      {
        string s = packet.ReadString(length);
        MySqlByte b = new MySqlByte(SByte.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture));
        b.TreatAsBoolean = TreatAsBoolean;
        return b;
      }
    }

    void IMySqlValue.SkipValue(MySqlPacket packet)
    {
      packet.ReadByte();
    }

    #endregion

    internal bool TreatAsBoolean
    {
      get { return treatAsBool; }
      set { treatAsBool = value; }
    }

    internal static void SetDSInfo(MySqlSchemaCollection sc)
    {
      // we use name indexing because this method will only be called
      // when GetSchema is called for the DataSourceInformation 
      // collection and then it wil be cached.
      MySqlSchemaRow row = sc.AddRow();
      row["TypeName"] = "TINYINT";
      row["ProviderDbType"] = MySqlDbType.Byte;
      row["ColumnSize"] = 0;
      row["CreateFormat"] = "TINYINT";
      row["CreateParameters"] = null;
      row["DataType"] = "System.SByte";
      row["IsAutoincrementable"] = true;
      row["IsBestMatch"] = true;
      row["IsCaseSensitive"] = false;
      row["IsFixedLength"] = true;
      row["IsFixedPrecisionScale"] = true;
      row["IsLong"] = false;
      row["IsNullable"] = true;
      row["IsSearchable"] = true;
      row["IsSearchableWithLike"] = false;
      row["IsUnsigned"] = false;
      row["MaximumScale"] = 0;
      row["MinimumScale"] = 0;
      row["IsConcurrencyType"] = DBNull.Value;
      row["IsLiteralSupported"] = false;
      row["LiteralPrefix"] = null;
      row["LiteralSuffix"] = null;
      row["NativeDataType"] = null;
    }
  }
}
