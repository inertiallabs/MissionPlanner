using uint8_t = System.Byte;
using uint16_t = System.UInt16;
using uint32_t = System.UInt32;
using uint64_t = System.UInt64;

using int8_t = System.SByte;
using int16_t = System.Int16;
using int32_t = System.Int32;
using int64_t = System.Int64;

using float32 = System.Single;

using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace DroneCAN
{
    public partial class DroneCAN 
    {
        public partial class uavcan_equipment_gnss_Auxiliary: IDroneCANSerialize 
        {
            public const int UAVCAN_EQUIPMENT_GNSS_AUXILIARY_MAX_PACK_SIZE = 16;
            public const ulong UAVCAN_EQUIPMENT_GNSS_AUXILIARY_DT_SIG = 0x9BE8BDC4C3DBBFD2;
            public const int UAVCAN_EQUIPMENT_GNSS_AUXILIARY_DT_ID = 1061;

            public Single gdop = new Single();
            public Single pdop = new Single();
            public Single hdop = new Single();
            public Single vdop = new Single();
            public Single tdop = new Single();
            public Single ndop = new Single();
            public Single edop = new Single();
            public uint8_t sats_visible = new uint8_t();
            public uint8_t sats_used = new uint8_t();

            public void encode(dronecan_serializer_chunk_cb_ptr_t chunk_cb, object ctx, bool fdcan = false)
            {
                encode_uavcan_equipment_gnss_Auxiliary(this, chunk_cb, ctx, fdcan);
            }

            public void decode(CanardRxTransfer transfer, bool fdcan = false)
            {
                decode_uavcan_equipment_gnss_Auxiliary(transfer, this, fdcan);
            }

            public static uavcan_equipment_gnss_Auxiliary ByteArrayToDroneCANMsg(byte[] transfer, int startoffset, bool fdcan = false)
            {
                var ans = new uavcan_equipment_gnss_Auxiliary();
                ans.decode(new DroneCAN.CanardRxTransfer(transfer.Skip(startoffset).ToArray()), fdcan);
                return ans;
            }
        }
    }
}