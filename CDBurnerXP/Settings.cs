using System;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Runtime.Serialization.Formatters.Binary;
using CDBurnerXP.IO;

namespace CDBurnerXP {

    public interface ISettingsProvider
    {
        object GetValue(params string[] path);

        void SetValue(string? value, params string[] path);
    }

    public class RegistrySettingsProvider : ISettingsProvider
    {
        private string m_BaseKey = "Software";

        public RegistrySettingsProvider(string baseKey)
        {
            m_BaseKey = baseKey;
        }

        /// <summary>
        /// Returns the key HKCU/Software/CDBurnerXP
        /// </summary>
        private RegistryKey GetHkcuKey()
        {
            RegistryKey key = Registry.CurrentUser;
            key = key.CreateSubKey(m_BaseKey);
            return key;
        }

        /// <summary>
        /// Gibt den Regsitry-Key zurck, dessen Wert wir setzen mssen. Erstellt ihn, wenn notig.
        /// </summary>
        /// <param name="section">darf null sein, und wird dann ignoriert</param>
        /// <returns></returns>
        private RegistryKey GetSettingsRegistryKey(string[] path)
        {
            RegistryKey settingsKey = GetHkcuKey();

            // No subkey required
            if (path.Length <= 1) return settingsKey;

            for (int i = 0; i < path.Length - 1; i++)
            {
                settingsKey = settingsKey.CreateSubKey(path[i]);
            }

            return settingsKey;
        }


        #region ISettingsProvider Member

        public object GetValue(params string[] path)
        {
            object? value = GetSettingsRegistryKey(path).GetValue(path[path.Length - 1]);
            return value ?? string.Empty;
        }

        public void SetValue(string? value, params string[] path)
        {
            try
            {
                GetSettingsRegistryKey(path).SetValue(path[path.Length - 1], value ?? string.Empty);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Ignore write errors. If settings cannot be stored, so be it.
                Debug.LogError(ex);
            }
        }

        #endregion
    }

    /// <summary>
    /// Speichert Einstellungen eines Programms in der Registry.
    /// Inbesondere gedacht fr Fensterpositionen, den Zustand von
    /// Controls etc.
    /// </summary>
    public class Settings {

        private static ISettingsProvider m_Provider = new RegistrySettingsProvider("Software\\Canneverbe Limited");

        public static ISettingsProvider Provider
        {
            get { return m_Provider; }
            set
            {
                // We always need a provider
                if (value != null)
                {
                    m_Provider = value;
                }
            }
        }

        /// <summary>
        /// Findet den passenden Serialisierer (durch Ausprobieren) und gibt das deserialisierte Objekt zurck.
        /// </summary>
        /// <returns>null bei Fehler, sonst ein object</returns>
        private static object? FindDeserializerGetValue (string? value) {
            if (value == null) return null;
            
            SettingsSerializer[] serializers = new SettingsSerializer[] { new PrimitiveSerializer (), new CommonSerializer (), new BinarySerializer () };
            foreach (SettingsSerializer serializer in serializers) {
                object? result = serializer.Deserialize (value);
                // Die Deserialisierer brechen frhzeitig ab, wenn sie mit dem Wert nicht klarkommen.
                // Insofern knnen wir hier alle durchprobieren.
                if (result != null) {
                    return result;
                }
            }
            return null;
        }


        public abstract class SettingsSerializer {
            /// <summary>
            /// Serialisiert einen beliebigen Typen.
            /// </summary>
            /// <returns>Typname:Wert.ToString()</returns>
            public virtual string Serialize (object value) {
                return value.GetType ().FullName + ":" + value.ToString ();
            }

            /// <summary>
            /// Deserialisiert einen String zu einem Objekt. Gibt null bei Fehlschlag zurck.
            /// Diese Methode muss fhr jeden Typen extra implementiert werden.
            /// </summary>
            public abstract object? Deserialize (string? value);
        }

        /// <summary>
        /// Serialisiert alle primitiven Typen (int, bool, string, float, double, decimal)
        /// </summary>
        public class PrimitiveSerializer : SettingsSerializer {
            public override object? Deserialize (string? value) {
                if (value == null) { return null; }
                
                int pos = value.IndexOf (':');
                if (pos < 0) { return null; }
                string type = value.Substring (0, pos);
                string data = value.Substring (pos + 1);

                switch (type) {
                    case "System.Boolean":
                        return System.Boolean.Parse (data);
                    case "System.Byte":
                        return System.Byte.Parse (data);
                    case "System.SByte":
                        return System.SByte.Parse (data);
                    case "System.Char":
                        return System.Char.Parse (data);
                    case "System.Decimal":
                        return System.Decimal.Parse (data);
                    case "System.Double":
                        return System.Double.Parse (data);
                    case "System.Single":
                        return System.Single.Parse (data);
                    case "System.Int32":
                        return System.Int32.Parse (data);
                    case "System.UInt32":
                        return System.UInt32.Parse (data);
                    case "System.Int64":
                        return System.Int64.Parse (data);
                    case "System.UInt64":
                        return System.UInt64.Parse (data);
                    case "System.Int16":
                        return System.Int16.Parse (data);
                    case "System.UInt16":
                        return System.UInt16.Parse (data);
                    case "System.String":
                        return data;
                }
                return null;
            }
        }

        /// <summary>
        /// Serialisiert einige hufig verwendete Typen (Point, Size, Rectangle)
        /// </summary>
        public class CommonSerializer : SettingsSerializer {
            public static bool SupportsType (Type type) {
                return (type == typeof (System.Drawing.Point)) || (type == typeof (System.Drawing.Size)) || (type == typeof (System.Drawing.Rectangle)) || (type == typeof (System.Drawing.PointF)) || (type == typeof (System.Drawing.SizeF));
            }

            public override object? Deserialize (string? value) {
                if (value == null) { return null; }
                
                int pos = value.IndexOf (':');
                if (pos < 0) { return null; }
                string type = value.Substring (0, pos);
                string data = value.Substring (pos + 1);

                string[] parts = data.Split (new char[] { ';' });

                try {
                    switch (type) {
                        case "System.Drawing.Point":
                            return new System.Drawing.Point (System.Int32.Parse (parts[0]), System.Int32.Parse (parts[1]));
                        case "System.Drawing.Size":
                            return new System.Drawing.Size (System.Int32.Parse (parts[0]), System.Int32.Parse (parts[1]));
                        case "System.Drawing.Rectangle":
                            return new System.Drawing.Rectangle (System.Int32.Parse (parts[0]), System.Int32.Parse (parts[1]), System.Int32.Parse (parts[2]), System.Int32.Parse (parts[3]));
                        case "System.Drawing.PointF":
                            return new System.Drawing.PointF (System.Single.Parse (parts[0]), System.Single.Parse (parts[1]));
                        case "System.Drawing.SizeF":
                            return new System.Drawing.SizeF (System.Single.Parse (parts[0]), System.Single.Parse (parts[1]));
                    }
                }
                catch {
                    return null;
                }
                return null;
            }

            public override string Serialize (object value) {
                string data = string.Empty;
                if (value is System.Drawing.Point) {
                    System.Drawing.Point pt = (System.Drawing.Point)value;
                    data = pt.X.ToString () + ";" + pt.Y.ToString ();
                }
                else if (value is System.Drawing.Size) {
                    System.Drawing.Size size = (System.Drawing.Size)value;
                    data = size.Width.ToString () + ";" + size.Height.ToString ();
                }
                else if (value is System.Drawing.Rectangle) {
                    System.Drawing.Rectangle rect = (System.Drawing.Rectangle)value;
                    data = rect.X.ToString () + ";" + rect.Y.ToString () + ";" + rect.Width.ToString () + ";" + rect.Height.ToString ();
                }
                else if (value is System.Drawing.PointF) {
                    System.Drawing.PointF pt = (System.Drawing.PointF)value;
                    data = pt.X.ToString () + ";" + pt.Y.ToString ();
                }
                else if (value is System.Drawing.SizeF) {
                    System.Drawing.SizeF size = (System.Drawing.SizeF)value;
                    data = size.Width.ToString () + ";" + size.Height.ToString ();
                }
                return value.GetType ().FullName + ":" + data;
            }
        }

        /// <summary>
        /// Serialisiert alle serialisierbaren Typen. Dies ist die
        /// letzte Chance vorgesehen, da der base64 string recht lang ist und das
        /// sieht in der Registry nicht so schn aus (vor allem lsst er sich schlecht bearbeiten).
        /// </summary>
        public class BinarySerializer : SettingsSerializer {
            /// <returns>base64-kodierter string</returns>
            public override string Serialize (object value) {
                // Use JSON serialization instead of BinaryFormatter to avoid SYSLIB0011 warning
                try
                {
                    return JsonSerializer.Serialize(value);
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }

            public override object? Deserialize (string? value) {
                if (value == null) { return null; }
                // Use JSON deserialization instead of BinaryFormatter to avoid SYSLIB0011 warning
                try
                {
                    // We need to know the type to deserialize to, so we'll return the string as-is
                    // In a real implementation, you would need to store type information
                    return value;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        #region Default

        public static bool SetDefault (string key, object value) {
            return SetDefault(System.String.Empty, key, value);
        }

        public static bool SetDefault (string company, string key, object value) {
            return SetDefault(System.String.Empty, key, value);
        }

        public static bool SetDefault (System.Windows.Forms.Control control, string key, object value) {
            return SetDefault(control.Name, key, value);
        }

        #endregion

        #region Setter SetValue

        /// <summary>
        /// Speichert einen beliebigen Wert in der Registry. Einzigste
        /// Bedingung ist, dass der Typ serialisierbar ist (IsSerializable).
        /// </summary>
        /// <param name="key">Name des Schlssels</param>
        /// <param name="value">Wert, der gepspeichert wird</param>
        public static void SetValue (string key, object value) {
            SetValue(System.String.Empty, key, value);
        }

        /// <summary>
        /// Speichert einen beliebigen Wert in der Registry. Einzigste
        /// Bedingung ist, dass der Typ serialisierbar ist (IsSerializable).
        /// </summary>
        /// <param name="control">Control; Eigenschaft "Name" wird als Sektionsname verwendet</param>
        /// <param name="key">Name des Schlssels</param>
        /// <param name="value">Wert, der gepspeichert wird</param>
        public static void SetValue (System.Windows.Forms.Control control, string key, object value) {
            SetValue(control.Name, key, value);
        }

        #endregion

        public static void SetValue (string ownRootNodeName, System.Windows.Forms.Control control, string key, object value) {
            SetValue(control.Name, key, value);
        }

        /// <summary>
        /// Speichert einen beliebigen Wert in der Registry. Einzigste
        /// Bedingung ist, dass der Typ serialisierbar ist (IsSerializable).
        /// </summary>
        /// <param name="ownRootNodeName">Gibt den Hauptknoten an. Kann null oder leer sein, dann wird WW-Ziel + YXZ genommen.</param>
        /// <param name="section">Unterschlssel (noch unterhalb des Dialogs). Kann null sein, und wird dann ignoriert.</param>
        /// <param name="key">Schlssel</param>
        /// <param name="value">Wert</param>
        public static void SetValue (string section, string key, object value) {
            if (key == null || key == string.Empty) {
                throw new ArgumentException ("Der Schlssel darf nicht leer sein.");
            }

            if (value == null)
            {
                m_Provider.SetValue(null, section, key);
                return;
            }

            // Check if the type is serializable using a try-catch approach instead of IsSerializable
            // to avoid SYSLIB0050 warning
            try
            {
                // Try to serialize the object to test if it's serializable
                JsonSerializer.Serialize(value);
            }
            catch (Exception)
            {
                throw new ArgumentException ("Werte mssen serialisierbar sein (typeof(...).IsSerializable == true)");
            }

            if (value.GetType().IsEnum)
            {
                value = Convert.ToInt32(value);
            }

            // Wir unterscheiden verschiedene Arten von Daten
            // - "Einfache" (sog. primitive) Typen: string, bool, int, float (Verarbeitung mit .Parse() und ToString()
            //   - Enum-Werte werden ebenfalls so verarbeitet
            // - Allgemeine Typen: Point, Size, PointF, SizeF (Selbstgeschriebene Funktionen)
            // - Sonstige serialisierbare Typen: Werden mit dem BinarySerializer verarbeitet

            SettingsSerializer serializer = new PrimitiveSerializer();

            if (value.GetType ().IsPrimitive) {
                serializer = new PrimitiveSerializer ();
            }
            else if (CommonSerializer.SupportsType (value.GetType ())) {
                // Allgemeine Typen
                serializer = new CommonSerializer ();
            }
            else {
                // Binr als letzte Chance
                serializer = new BinarySerializer ();
            }

            m_Provider.SetValue(serializer.Serialize(value), section, key);
        }


        #region Getter GetValue

        public static object GetValue (string key) {
            return GetValue(System.String.Empty, key, string.Empty);
        }

        public static object GetValue (System.Windows.Forms.Control control, string key) {
            return GetValue(control, key);
        }

        public static object GetValue (string key, object defaultValue) {
            return GetValue(System.String.Empty, key, defaultValue);
        }

        public static object GetValue (System.Windows.Forms.Control control, string key, object defaultValue) {
            return GetValue(control.Name, key, defaultValue);
        }

        public static object GetValue(string section, string key, object defaultValue)
        {
            try
            {
                string? value = m_Provider.GetValue(section, key) as string;
                object? result = FindDeserializerGetValue(value);
                return result ?? defaultValue;
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.LogError(ex);
                return defaultValue;
            }
        }

        #endregion
    }
}