namespace BytesRoad.Net.Sockets
{
    using System;

    internal class ByteVector
    {
        private int _capacity;
        private byte[] _data = new byte[0];
        private int _size;

        internal ByteVector()
        {
        }

        internal void Add(ByteVector data)
        {
            this.Add(data.Data, 0, data.Size);
        }

        internal void Add(byte[] data)
        {
            this.EnsureSpace(data.Length);
            Array.Copy(data, 0, this._data, this._size, data.Length);
            this._size += data.Length;
        }

        internal void Add(byte[] data, int offset)
        {
            int needMore = data.Length - offset;
            this.EnsureSpace(needMore);
            Array.Copy(data, offset, this._data, this._size, needMore);
            this._size += needMore;
        }

        internal void Add(byte[] data, int offset, int length)
        {
            this.EnsureSpace(length);
            Array.Copy(data, offset, this._data, this._size, length);
            this._size += length;
        }

        internal void Clear()
        {
            this._size = 0;
        }

        internal void CutHead(int count)
        {
            if (count < 0)
            {
                throw new ArgumentException("Should be a positive value", "count");
            }
            if (count > this._size)
            {
                this._size = 0;
            }
            else
            {
                this._size -= count;
                Array.Copy(this._data, count, this._data, 0, this._size);
            }
        }

        internal void CutTail(int count)
        {
            if (count < 0)
            {
                throw new ArgumentException("Should be a positive value", "count");
            }
            if (count > this._size)
            {
                this._size = 0;
            }
            else
            {
                this._size -= count;
            }
        }

        private void EnsureSpace(int needMore)
        {
            if ((this._size + needMore) >= this._capacity)
            {
                this.Reallocate(this._size + needMore);
            }
        }

        private void Reallocate(int requiredSize)
        {
            int num = (this._capacity > 0) ? this._capacity : 1;
            while (num < requiredSize)
            {
                num = num << 1;
            }
            byte[] array = new byte[num];
            if (this._data != null)
            {
                this._data.CopyTo(array, 0);
            }
            this._data = array;
            this._capacity = num;
        }

        internal int Capacity
        {
            get
            {
                return this._capacity;
            }
        }

        internal byte[] Data
        {
            get
            {
                return this._data;
            }
        }

        internal int Size
        {
            get
            {
                return this._size;
            }
        }
    }
}

