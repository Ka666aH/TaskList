export function toDisplayDate(iso?: string | null): string {
  if (!iso) return ''
  const truncated = iso.replace(/\.\d+/, '')
  const date = new Date(truncated)
  if (Number.isNaN(date.getTime())) return iso
  return date.toLocaleString()
}
export function toLocalInput(iso: string | null | undefined): string {
  if (!iso) return ''
  const d = new Date(iso)
  if (Number.isNaN(d.getTime())) return ''
  const pad = (n: number) => String(n).padStart(2, '0')
  return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}T${pad(d.getHours())}:${pad(d.getMinutes())}`
}
export function toIso(local: string | null | undefined): string | null {
  if (!local) return null
  const d = new Date(local)
  if (Number.isNaN(d.getTime())) return null
  return d.toISOString()
}
